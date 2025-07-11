using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UVACanvasAccess.Model.Files;
using UVACanvasAccess.Structures.Files;
using UVACanvasAccess.Util;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Structures.Discussions;

namespace UVACanvasAccess.ApiParts {
    
    public partial class Api {
        /// <summary>
        /// Performs a file upload to any Canvas endpoint that accepts file uploads.
        /// This method should be called by a specific endpoint method and not directly.
        /// </summary>
        /// <param name="endpoint">The endpoint to upload to.</param>
        /// <param name="file">The data.</param>
        /// <param name="fileName">The file's name without its extension.</param>
        /// <param name="filePath">The file's path on the local machine.</param>
        /// <param name="parentFolderId">The id of the folder to place the file in.</param>
        /// <param name="parentFolderPath">The path of the folder to place the file in.</param>
        /// <param name="onDuplicate">How to handle a duplicate filename. Can be <c>overwrite</c> or <c>rename</c>.
        /// The default is <c>overwrite</c>. Not applicable in a context where files are not placed in folders.
        /// </param>
        /// <param name="contentType">The MIME type of the file. If absent, it will be determined by calling
        /// <see cref="MimeMapping.GetMimeMapping"/> on the filePath.
        /// </param>
        /// <returns>The uploaded file.</returns>
        /// <exception cref="Exception"></exception>
        /// <remarks>The request will fail if both <c>parentFolderId</c> and <c>parentFolderPath</c> are supplied.</remarks>
        /// <see href="https://canvas.instructure.com/doc/api/file.file_uploads.html"/>
        private async Task<CanvasFile> UploadFile(string endpoint, byte[] file, string fileName, string filePath, 
                                                  string parentFolderId = null, string parentFolderPath = null, 
                                                  string onDuplicate = null, string contentType = null) {

            if (contentType == null) {
                contentType = MimeMapping.GetMimeMapping(filePath);
            }

            var firstPostArgs = BuildHttpArguments(new[] {
                                                              ("name", fileName),
                                                              ("size", file.Length.ToString()),
                                                              ("content_type", contentType),
                                                              ("parent_folder_path", parentFolderPath),
                                                              ("parent_folder_id", parentFolderId),
                                                              ("on_duplicate", onDuplicate)
                                                          });

            var firstPostResponse = await client.PostAsync(endpoint, firstPostArgs);

            firstPostResponse.AssertSuccess();

            var firstResponseJson = JObject.Parse(await firstPostResponse.Content.ReadAsStringAsync());
            var uploadUrl = firstResponseJson["upload_url"].ToString();
            var uploadParams = (JObject) firstResponseJson["upload_params"];

            var uploadParamsList = uploadParams.Properties().Select(kv => (kv.Name, kv.Value.ToString()));

            var secondPostArgs = BuildHttpArguments(uploadParamsList.Append(("file", fileName)));
            
            var bytesContent = new ByteArrayContent(file);
            bytesContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            
            var secondPostData = new MultipartFormDataContent {
                                                                  secondPostArgs,
                                                                  { bytesContent, fileName, filePath }
                                                              };
            
            var secondPostResponse = await client.PostAsync(uploadUrl, secondPostData);

            secondPostResponse.AssertSuccess();


            CanvasFileModel model;
            if (secondPostResponse.StatusCode != HttpStatusCode.MovedPermanently) {
                model = JsonConvert.DeserializeObject<CanvasFileModel>(await secondPostResponse.Content.ReadAsStringAsync());
            } else {
                var thirdResponse = await client.GetAsync(secondPostResponse.Headers.Location);
                thirdResponse.AssertSuccess();
                model = JsonConvert.DeserializeObject<CanvasFileModel>(await thirdResponse.Content.ReadAsStringAsync());
            }
            
            return new CanvasFile(this, model);

        }

        /// <summary>
        /// Uploads a file to the current user's personal files section.
        /// </summary>
        /// <param name="file">The data.</param>
        /// <param name="filePath">The URI of the file on the local system.
        /// Must include, at a minimum, the name and the extension.
        /// </param>
        /// <param name="parentFolderName"></param>
        /// <returns>The uploaded file.</returns>
        public Task<CanvasFile> UploadPersonalFile(byte[] file, 
                                                   string filePath,
                                                   string parentFolderName = null) {
            return UploadFile("users/self/files", 
                              file, 
                              Path.GetFileNameWithoutExtension(filePath), 
                              Path.GetFileName(filePath),
                              parentFolderPath: parentFolderName
                             );
        }

        internal Task<byte[]> DownloadPersonalFile(CanvasFile cf) {
            return client.GetByteArrayAsync(cf.Url);
        }

        internal Task<byte[]> DownloadFileAttachment(FileAttachment fa) {
            return client.GetByteArrayAsync(fa.Url);
        }

        internal Task<Stream> StreamFileAttachment(FileAttachment fa) {
            return client.GetStreamAsync(fa.Url);
        }

        /// <summary>
        /// Create a folder in the current user's personal files section.
        /// </summary>
        /// <param name="name">The folder name.</param>
        /// <param name="parentFolderName">(Optional) The path to create the folder in. By default, root.</param>
        /// <param name="lockDate">(Optional) The date to lock the folder on.</param>
        /// <param name="unlockDate">(Optional) The date to unlock the folder on.</param>
        /// <param name="locked">(Optional) Lock the folder.</param>
        /// <param name="hidden">(Optional) Hide the folder.</param>
        /// <param name="position">(Optional) The folder's position in the list of folders.</param>
        /// <returns>The new folder.</returns>
        public async Task<Folder> CreatePersonalFolder(string name,
                                                       string parentFolderName = null,
                                                       DateTime? lockDate = null,
                                                       DateTime? unlockDate = null,
                                                       bool? locked = null,
                                                       bool? hidden = null,
                                                       int? position = null) {
            var args = BuildHttpArguments(new [] {
                ("name", name),
                ("parent_folder_path", parentFolderName),
                ("lock_at", lockDate?.ToIso8601Date()),
                ("unlock_at", unlockDate?.ToIso8601Date()),
                ("locked", locked?.ToShortString()),
                ("hidden", hidden?.ToShortString()),
                ("position", position?.ToString())
            });
            var response = await client.PostAsync("users/self/folders", args);

            var model = JsonConvert.DeserializeObject<FolderModel>(await response.Content.ReadAsStringAsync());
            return new Folder(this, model);
        }

        /// <summary>
        /// Get a single personal folder for the current user.
        /// </summary>
        /// <param name="folderId">(Optional) The folder id. By default, root.</param>
        /// <returns>The folder.</returns>
        public async Task<Folder> GetPersonalFolder(ulong? folderId) {
            var folder = folderId?.ToString() ?? "root";
            var response = await client.GetAsync($"users/self/folders/{folder}");

            var model = JsonConvert.DeserializeObject<FolderModel>(await response.Content.ReadAsStringAsync());
            return new Folder(this, model);
        }
        
        internal async IAsyncEnumerable<Folder> StreamFoldersBase(string requestPath) {
            var response = await client.GetAsync(requestPath + BuildQueryString());

            await foreach (var model in StreamDeserializePages<FolderModel>(response)) {
                yield return new Folder(this, model);
            }
        }

        /// <summary>
        /// Stream the current user's personal folders.
        /// </summary>
        /// <returns>The stream of folders.</returns>
        public IAsyncEnumerable<Folder> StreamPersonalFolders() {
            return StreamFoldersBase("users/self/folders");
        }

        /// <summary>
        /// Stream the folders in a specific <see cref="Folder"/>.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <returns>The stream of folders.</returns>
        public IAsyncEnumerable<Folder> StreamFoldersInFolder(ulong folderId) {
            return StreamFoldersBase($"folders/{folderId}/folders");
        }

        internal async IAsyncEnumerable<CanvasFile> StreamFilesBase(string requestPath,
                                                                    IEnumerable<ContentType> includeContentTypes = null,
                                                                    IEnumerable<ContentType> excludeContentTypes = null,
                                                                    string searchTerm = null,
                                                                    FileIncludes? includes = null,
                                                                    bool onlyIncludeFileNames = false,
                                                                    FileSort? sortBy = null,
                                                                    Order? order = null) {
            var args = new List<(string, string)>();

            if (includeContentTypes != null) {
                args.AddRange(includeContentTypes.Select(ct => ct.MediaType)
                                                 .Select(mt => ("content_types[]", mt)));
            }

            if (excludeContentTypes != null) {
                args.AddRange(
                    excludeContentTypes
                        .Select(ct => ct.MediaType)
                        .Select(mt => ("exclude_content_types[]", mt))
                );
            }

            if (searchTerm != null) {
                args.Add(("search_term", searchTerm));
            }

            if (includes != null) {
                args.AddRange(
                    includes.GetFlagsApiRepresentations().Select(r => ("include[]", r))
                );
            }

            if (onlyIncludeFileNames) {
                args.Add(("only[]", "names"));
            }

            if (sortBy != null) {
                args.Add(("sort", sortBy.GetApiRepresentation()));
            }

            if (order != null) {
                args.Add(("order", order.GetApiRepresentation()));
            }
            
            var response = await client.GetAsync(requestPath + BuildDuplicateKeyQueryString(args.ToArray()));

            await foreach (var model in StreamDeserializePages<CanvasFileModel>(response)) {
                yield return new CanvasFile(this, model);
            }
        }

        /// <summary>
        /// Stream the current user's personal files.
        /// </summary>
        /// <param name="includeContentTypes">(Optional) Content types to include.</param>
        /// <param name="excludeContentTypes">(Optional) Content type to exclude.</param>
        /// <param name="searchTerm">(Optional) A search term.</param>
        /// <param name="includes">(Optional) Extra data to include with the results.</param>
        /// <param name="onlyIncludeFileNames">(Optional) Only include file names with the results.</param>
        /// <param name="sortBy">(Optional) The category to sort the results by.</param>
        /// <param name="order">(Optional) The order to sort the results by.</param>
        /// <returns>The stream of files.</returns>
        public IAsyncEnumerable<CanvasFile> StreamPersonalFiles(IEnumerable<ContentType> includeContentTypes = null, 
                                                                IEnumerable<ContentType> excludeContentTypes = null, 
                                                                string searchTerm = null, 
                                                                FileIncludes? includes = null, 
                                                                bool onlyIncludeFileNames = false,
                                                                FileSort? sortBy = null,
                                                                Order? order = null) {
            return StreamFilesBase("users/self/files", includeContentTypes, excludeContentTypes, searchTerm, includes, onlyIncludeFileNames, sortBy, order);
        }

        /// <summary>
        /// Stream the files in a specific <see cref="Folder"/>.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="includeContentTypes">(Optional) Content types to include.</param>
        /// <param name="excludeContentTypes">(Optional) Content type to exclude.</param>
        /// <param name="searchTerm">(Optional) A search term.</param>
        /// <param name="includes">(Optional) Extra data to include with the results.</param>
        /// <param name="onlyIncludeFileNames">(Optional) Only include file names with the results.</param>
        /// <param name="sortBy">(Optional) The category to sort the results by.</param>
        /// <param name="order">(Optional) The order to sort the results by.</param>
        /// <returns>The stream of files.</returns>
        public IAsyncEnumerable<CanvasFile> StreamFilesInFolder(ulong folderId,
                                                                IEnumerable<ContentType> includeContentTypes = null,
                                                                IEnumerable<ContentType> excludeContentTypes = null,
                                                                string searchTerm = null,
                                                                FileIncludes? includes = null,
                                                                bool onlyIncludeFileNames = false,
                                                                FileSort? sortBy = null,
                                                                Order? order = null) {
            return StreamFilesBase($"folders/{folderId}/files", includeContentTypes, excludeContentTypes, searchTerm, includes, onlyIncludeFileNames, sortBy, order);
        }

        /// <summary>
        /// Get a single personal file for the current user.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="includes">(Optional) Extra data to include with the result.</param>
        /// <returns>The file.</returns>
        public async Task<CanvasFile> GetPersonalFile(ulong fileId, FileIncludes? includes = null) {
            var response = await client.GetAsync($"files/{fileId}" + (includes == null ? "" 
                                                                           : BuildDuplicateKeyQueryString(includes.GetFlagsApiRepresentations().Select(r => ("include[]", r)).ToArray())));
            var model = JsonConvert.DeserializeObject<CanvasFileModel>(await response.Content.ReadAsStringAsync());
            return new CanvasFile(this, model);
        }

        /// <summary>
        /// Retrieve a hierarchy of personal folders from a path.
        /// </summary>
        /// <param name="parts">Each folder in the path, in order.</param>
        /// <returns>The list of folders, starting from root and ending with the deepest subfolder.</returns>
        /// <exception cref="DoesNotExistException">If the path given does not exist or is not visible.</exception>
        public async Task<IEnumerable<Folder>> ResolvePersonalFolderPath(params string[] parts) {
            var path = string.Join("/", parts);
            var response = await client.GetAsync("users/self/folders/by_path/" + path + BuildQueryString())
                                        .ThenApplyAwait(async r => await r.Content.ReadAsStringAsync())
                                        .ThenApply(JToken.Parse)
                                        .ThenApply(jt => jt.CheckError());
            if (response.IsT1) {
                throw new DoesNotExistException(response.AsT1);
            }

            return response.AsT0.ToObject<IEnumerable<FolderModel>>()
                                .Select(fm => new Folder(this, fm));
        }

        /// <summary>
        /// Determine if a personal folder exists at the given path.
        /// </summary>
        /// <param name="parts">Each folder in the path, in order.</param>
        /// <returns>Whether or not the personal folder exists.</returns>
        public async Task<bool> PersonalFolderPathExists(params string[] parts) {
            var path = string.Join("/", parts);
            var response = await client.GetAsync("users/self/folders/by_path/" + path + BuildQueryString())
                                        .ThenApplyAwait(async r => await r.Content.ReadAsStringAsync())
                                        .ThenApply(JToken.Parse)
                                        .ThenApply(jt => jt.CheckError());
            return response.IsT0;
        }

        /// <summary>
        /// Categories of optional data that can be requested for inclusion within <see cref="CanvasFile"/> objects.
        /// </summary>
        [PublicAPI]
        [Flags]
        public enum FileIncludes : byte {
            /// <summary>
            /// Include the file's owner as a complete user object.
            /// </summary>
            [ApiRepresentation("user")]
            User = 1 << 0,
            /// <summary>
            /// Include usage rights/licensing information for the file.
            /// </summary>
            [ApiRepresentation("usage_rights")]
            UsageRights = 1 << 1
        }

        /// <summary>
        /// Categories a <see cref="CanvasFile"/> can be sorted by.
        /// </summary>
        [PublicAPI]
        public enum FileSort : byte {
            /// <summary>
            /// Sort by name.
            /// </summary>
            [ApiRepresentation("name")]
            Name,
            /// <summary>
            /// Sort by size.
            /// </summary>
            [ApiRepresentation("size")]
            Size,
            /// <summary>
            /// Sort by creation time.
            /// </summary>
            [ApiRepresentation("created_at")]
            CreatedAt,
            /// <summary>
            /// Sort by update time.
            /// </summary>
            [ApiRepresentation("updated_at")]
            UpdatedAt,
            /// <summary>
            /// Sort by <see cref="CanvasFile.ContentType">content type</see>.
            /// </summary>
            [ApiRepresentation("content_type")]
            ContentType,
            /// <summary>
            /// Sort by the file owner.
            /// </summary>
            [ApiRepresentation("user")]
            User
        }
        
        /// <summary>
        /// Contains the total storage quota and quota usage for a user.
        /// </summary>
        [PublicAPI]
        public readonly struct QuotaInfo<T> where T: struct {
            /// <summary>
            /// The total storage quota.
            /// </summary>
            public T TotalQuota { get; }
            
            /// <summary>
            /// The amount of quota currently used.
            /// </summary>
            public T UsedQuota { get; }

            internal QuotaInfo(T totalQuota, T usedQuota) {
                TotalQuota = totalQuota;
                UsedQuota = usedQuota;
            }
            
            /// <summary>
            /// Deconstructs the <see cref="QuotaInfo{T}"/> into <see cref="TotalQuota"/> and <see cref="UsedQuota"/>.
            /// </summary>
            /// <param name="totalQuota">The total storage quota.</param>
            /// <param name="usedQuota">The amount of quota currently used.</param>
            public void Deconstruct(out T totalQuota, out T usedQuota) {
                totalQuota = TotalQuota;
                usedQuota = UsedQuota;
            }
        }

        /// <summary>
        /// Returns the storage quota in bytes of the current user, along with the amount currently used.
        /// </summary>
        /// <returns>The quota information.</returns>
        public async Task<QuotaInfo<ulong>> GetPersonalQuota() {
            var response = await client.GetAsync("users/self/files/quota" + BuildQueryString());

            var q = JObject.Parse(await response.Content.ReadAsStringAsync());
            return new QuotaInfo<ulong>(q["quota"].Value<ulong>(), q["quota_used"].Value<ulong>());
        }

        /// <summary>
        /// Returns the storage quota in MiB of the current user, along with the amount currently used.
        /// </summary>
        /// <returns>The quota information.</returns>
        public Task<QuotaInfo<decimal>> GetPersonalQuotaMiB() {
            const decimal mib = 1024 * 1024;
            return GetPersonalQuota().ThenApply(t => new QuotaInfo<decimal>(t.TotalQuota / mib, t.UsedQuota / mib));
        }

        /// <summary>
        /// Updates a personal file for the current user. All arguments except for <paramref name="fileId"/> are optional.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="lockAt">When the file should lock.</param>
        /// <param name="unlockAt">When the file should unlock.</param>
        /// <param name="locked">Whether or not the file is locked.</param>
        /// <param name="hidden">Whether or not the file is hidden.</param>
        /// <returns>The file.</returns>
        public async Task<CanvasFile> UpdatePersonalFile(ulong fileId,
                                                         DateTime? lockAt = null,
                                                         DateTime? unlockAt = null,
                                                         bool? locked = null,
                                                         bool? hidden = null) {
            var args = new[] {
                ("lock_at", lockAt?.ToIso8601Date()),
                ("unlock_at", unlockAt?.ToIso8601Date()),
                ("locked", locked?.ToShortString()),
                ("hidden", hidden?.ToShortString())
            };

            var response = await client.PutAsync($"files/{fileId}", BuildHttpArguments(args));

            var model = JsonConvert.DeserializeObject<CanvasFileModel>(await response.Content.ReadAsStringAsync());
            return new CanvasFile(this, model);
        }

        /// <summary>
        /// Updates a personal folder for the current user.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="name">The folder name.</param>
        /// <returns>The folder.</returns>
        public async Task<Folder> UpdatePersonalFolder(ulong folderId, string name) { // todo incomplete
            
            var args = new[] {
                ("name", name)
            };

            var response = await client.PutAsync($"folders/{folderId}", BuildHttpArguments(args));
            
            var model = JsonConvert.DeserializeObject<FolderModel>(await response.Content.ReadAsStringAsync());
            return new Folder(this, model);
        }

        /// <summary>
        /// Deletes a personal folder for the current user.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <param name="rf">(Optional) If true, the folder does not need to be empty.</param>
        /// <returns></returns>
        public async Task DeletePersonalFolder(ulong folderId, bool rf = false) {
            var args = new[] {
                ("force", rf.ToShortString())
            };

            await client.DeleteAsync($"folders/{folderId}" + BuildQueryString(args)).AssertSuccess();
        }

        /// <summary>
        /// Moves a personal file for the current user.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="onDuplicate">The action taken to resolve a duplicate.</param>
        /// <param name="name">(Optional) The new name. If none given, the previous name will be kept.</param>
        /// <param name="destinationFolderId">(Optional) The id of the folder to move the file into. If none give, the file will remain in place.</param>
        /// <returns>The file.</returns>
        public async Task<CanvasFile> MovePersonalFile(ulong fileId, 
                                                       OnDuplicate onDuplicate, 
                                                       string name = null, 
                                                       ulong? destinationFolderId = null) {
            var args = new[] {
                ("name", name),
                ("parent_folder_id", destinationFolderId.ToString()),
                ("on_duplicate", onDuplicate.GetApiRepresentation())
            };
            
            var response = await client.PutAsync($"files/{fileId}", BuildHttpArguments(args));

            var model = JsonConvert.DeserializeObject<CanvasFileModel>(await response.Content.ReadAsStringAsync());
            return new CanvasFile(this, model);
        }

        /// <summary>
        /// Copies a personal file for the current user.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="destinationFolderId">The id of the folder to copy the file into.</param>
        /// <param name="onDuplicate">The action taken to resolve a duplicate.</param>
        /// <returns>The new file.</returns>
        public async Task<CanvasFile> CopyPersonalFile(ulong fileId, ulong destinationFolderId, OnDuplicate onDuplicate) {
            var args = new[] {
                ("source_file_id", fileId.ToString()),
                ("on_duplicate", onDuplicate.GetApiRepresentation())
            };

            var response =
                await client.PostAsync($"folders/{destinationFolderId}/copy_file", BuildHttpArguments(args));
            
            var model = JsonConvert.DeserializeObject<CanvasFileModel>(await response.Content.ReadAsStringAsync());
            return new CanvasFile(this, model);
        }

        /// <summary>
        /// Deletes a personal file for the current user.
        /// </summary>
        /// <param name="fileId">The file id.</param>
        /// <param name="replace">(Optional) If true, the file and all generated previews will be replaced with a blank placeholder.</param>
        /// <returns>The file.</returns>
        public async Task<CanvasFile> DeletePersonalFile(ulong fileId, bool? replace = null) {
            var args = new[] {
                ("replace", replace?.ToShortString())
            };
            
            var response =
                await client.DeleteAsync($"files/{fileId}" + BuildQueryString(args));
            
            var model = JsonConvert.DeserializeObject<CanvasFileModel>(await response.Content.ReadAsStringAsync());
            return new CanvasFile(this, model);
        }
    }
}