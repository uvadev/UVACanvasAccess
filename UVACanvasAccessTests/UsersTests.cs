using System.Collections.Generic;
using System.Threading.Tasks;
using UVACanvasAccess.ApiParts;
using UVACanvasAccess.Exceptions;
using UVACanvasAccess.Structures.Users;
using Xunit;
using Xunit.Abstractions;

namespace UVACanvasAccessTests {
    
    public class UsersTests : IClassFixture<ApiFixture> {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly Api _api;

        public UsersTests(ApiFixture fixture, ITestOutputHelper testOutputHelper) {
            _testOutputHelper = testOutputHelper;
            _api = fixture.Api;
        }
        
        /// <summary>
        /// Tests GetUserDetails and GetProfile.
        /// </summary>
        [Theory]
        [InlineData(3390)]
        [InlineData(3392)]
        [InlineData(3394)]
        public async Task Test1(ulong testUser) {
            var user = await _api.GetUserDetails(testUser);

            Assert.Equal(testUser, user.Id);
            Assert.NotNull(user.Name);
            Assert.NotNull(user.SortableName);
            Assert.NotNull(user.ShortName);
            Assert.NotNull(user.AvatarUrl);
            Assert.NotNull(user.Permissions);
            Assert.NotNull(user.LoginId);
            
            
            var profile = await user.GetProfile();
            
            Assert.Equal(testUser, profile.Id);
            Assert.Equal(user.Name, profile.Name);
            Assert.Equal(user.SortableName, profile.SortableName);
            Assert.Equal(user.ShortName, profile.ShortName);
            Assert.Equal(user.LoginId, profile.LoginId);
            Assert.Equal(user.AvatarUrl, profile.AvatarUrl);
        }

        /// <summary>
        /// Tests that GetUserDetails throws the correct exception on failure.
        /// </summary>
        [Theory]
        [InlineData(2323232323232)]
        [InlineData(0)]
        public async Task Test2(ulong nonexistentId) {
            await Assert.ThrowsAsync<DoesNotExistException>(() => _api.GetUserDetails(nonexistentId));
        }

        /// <summary>
        /// Tests GetListUsers.
        /// </summary>
        [Theory]
        [InlineData("username", "asc")]
        [InlineData("username", "desc")]
        [InlineData("username", null)]
        [InlineData("email", "asc")]
        [InlineData("email", "desc")]
        [InlineData("email", null)]
        [InlineData("last_login", "asc")]
        [InlineData("last_login", "desc")]
        [InlineData("last_login", null)]
        [InlineData(null, null)]
        [InlineData(null, "asc")]
        [InlineData(null, "desc")]
        public async Task Test3(string sort, string order) {
            IEnumerable<User> users = await _api.GetListUsers("test", sort, order);
            Assert.NotEmpty(users);
        }
        
        
    }
}