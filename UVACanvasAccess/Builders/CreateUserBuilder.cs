using System.Collections.Generic;
using System.Threading.Tasks;
using UVACanvasAccess.Structures.Users;

namespace UVACanvasAccess.Builders {
    
    /// <summary>
    /// A class used to create new Users using the builder pattern.
    /// When all desired fields are set, call <see cref="Post"/> to create the user.
    /// </summary>
    public class CreateUserBuilder {
        private readonly Api _api;
        internal string AccountId { get; }
        internal Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        internal CreateUserBuilder(Api api, string accountId) {
            _api = api;
            AccountId = accountId;
        }

        /// <summary>
        /// The full name of the user. This name will be used by teacher for grading.
        /// Required if this is a self-registration.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithName(string name) {
            Fields["user[name]"] = name;
            return this;
        }

        /// <summary>
        /// The user's name as it will be displayed in discussions, messages, and comments.
        /// </summary>
        /// <param name="shortName"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithShortName(string shortName) {
            Fields["user[short_name]"] = shortName;
            return this;
        }

        /// <summary>
        /// The user's name as used to sort alphabetically in lists.
        /// </summary>
        /// <param name="sortableName"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithSortableName(string sortableName) {
            Fields["user[sortable_name]"] = sortableName;
            return this;
        }

        /// <summary>
        /// The time zone for the user. The allowed time zone formats are IANA time zones or Ruby on Rails time zones.
        /// </summary>
        /// <param name="timeZone"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithTimeZone(string timeZone) {
            Fields["user[time_zone]"] = timeZone;
            return this;
        }

        /// <summary>
        /// The user's preferred language, from the list of languages Canvas supports, in RFC-5646 format.
        /// </summary>
        /// <param name="locale"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithLocale(string locale) {
            Fields["user[locale]"] = locale;
            return this;
        }

        /// <summary>
        /// The user's birth date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithBirthDate(string date) {
            Fields["user[birthdate]"] = date;
            return this;
        }

        /// <summary>
        /// Whether the user accepts the terms of use.
        /// Required if this is a self-registration and this canvas instance requires users to accept the terms.
        /// </summary>
        /// <param name="hasAcceptedTerms"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithTermsOfUseAccepted(bool hasAcceptedTerms = true) {
            Fields["user[terms_of_use]"] = hasAcceptedTerms.ToString().ToLower();
            return this;
        }
        
        /// <summary>
        /// Automatically mark the user as registered.
        /// If this is true, it is recommended to set <see cref="WithSendConfirmation"/> to true as well.
        /// </summary>
        /// <param name="skipRegistration"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithSkipRegistration(bool skipRegistration = true) {
            Fields["user[skip_registration]"] = skipRegistration.ToString().ToLower();
            return this;
        }

        /// <summary>
        /// The user's login ID. If this is a self-registration, it must be a valid email address.
        /// Always required.
        /// </summary>
        /// <param name="uniqueId"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithUniqueId(string uniqueId) {
            Fields["pseudonym[unique_id]"] = uniqueId;
            return this;
        }

        /// <summary>
        /// The user's password. Cannot be set during self-registration.
        /// </summary>
        /// <param name="password"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithPassword(string password) {
            Fields["pseudonym[password]"] = password;
            return this;
        }

        /// <summary>
        /// SIS ID for the user's account.
        /// To set this parameter, the caller must be able to manage SIS permissions.
        /// </summary>
        /// <param name="sis"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithSisUserId(string sis) {
            Fields["pseudonym[sis_user_id]"] = sis;
            return this;
        }

        /// <summary>
        /// Integration ID for the login.
        /// To set this parameter, the caller must be able to manage SIS permissions.
        /// The Integration ID is a secondary identifier useful for more complex SIS integrations.
        /// </summary>
        /// <param name="integrationId"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithIntegrationId(string integrationId) {
            Fields["pseudonym[integration_id]"] = integrationId;
            return this;
        }

        /// <summary>
        /// Send user notification of account creation if true.
        /// Automatically set to true during self-registration.
        /// </summary>
        /// <param name="shouldSendConfirmation"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithSendConfirmation(bool shouldSendConfirmation = true) {
            Fields["pseudonym[send_confirmation]"] = shouldSendConfirmation.ToString().ToLower();
            return this;
        }

        /// <summary>
        /// Send the user a self-registration email.
        /// </summary>
        /// <param name="shouldForceSelfRegistration"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithForceSelfRegistration(bool shouldForceSelfRegistration = true) {
            Fields["pseudonym[force_self_registration]"] = shouldForceSelfRegistration.ToString().ToLower();
            return this;
        }

        /// <summary>
        /// The authentication provider this login is associated with.
        /// </summary>
        /// <param name="authProviderId"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithAuthProviderId(string authProviderId) {
            Fields["pseudonym[authentication_provider_id]"] = authProviderId;
            return this;
        }

        /// <summary>
        /// The communication channel type, e.g. 'email' or 'sms'.
        /// </summary>
        /// <param name="type"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithCommunicationChannelType(string type) {
            Fields["communication_channel[type]"] = type;
            return this;
        }

        /// <summary>
        /// The communication channel address, e.g. the user's email address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithCommunicationChannelAddress(string address) {
            Fields["communication_channel[address]"] = address;
            return this;
        }

        /// <summary>
        /// Only valid for account admins. If true, returns the new user account confirmation URL in the response.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithCommunicationChannelConfirmationUrl(bool returnUrl = true) {
            Fields["communication_channel[confirmation_url]"] = returnUrl.ToString().ToLower();
            return this;
        }

        /// <summary>
        /// Only valid for site admins and account admins making requests.
        /// If true, the channel is automatically validated and no confirmation email or SMS is sent.
        /// Otherwise, the user must respond to a confirmation message to confirm the channel.
        /// </summary>
        /// <param name="skip"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithSkipCommunicationChannelConfirmation(bool skip = true) {
            Fields["communication_channel[skip_confirmation]"] = skip.ToString().ToLower();
            return this;
        }

        /// <summary>
        /// If true, validations are performed on the newly created user (and their associated pseudonym),
        /// even if the request is made by a privileged user like an admin.
        /// </summary>
        /// <param name="force"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithForceValidations(bool force = true) {
            Fields["force_validations"] = force.ToString().ToLower();
            return this;
        }

        /// <summary>
        /// When true, will first try to re-activate a deleted user with matching sis_user_id if possible.
        /// </summary>
        /// <param name="enable"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithSisReactivation(bool enable = true) {
            Fields["enable_sis_reactivation"] = enable.ToString().ToLower();
            return this;
        }

        
        /// <summary>
        /// A canvas URL to be used as a redirect destination.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>This builder.</returns>
        public CreateUserBuilder WithDestination(string url) {
            Fields["destination"] = url;
            return this;
        }

        /// <summary>
        /// Creates a new user using the fields in this builder.
        /// </summary>
        /// <returns>The new user.</returns>
        public Task<User> Post() {
            return _api.PostCreateUser(this);
        }
    }
}