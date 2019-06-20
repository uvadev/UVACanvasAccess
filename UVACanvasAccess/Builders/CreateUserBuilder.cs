using System.Collections.Generic;
using System.Threading.Tasks;
using UVACanvasAccess.Structures.Users;

namespace UVACanvasAccess.Builders {
    public class CreateUserBuilder {
        private readonly Api _api;
        public string AccountId { get; }
        public Dictionary<string, string> Fields { get; } = new Dictionary<string, string>();

        public CreateUserBuilder(Api api, string accountId) {
            _api = api;
            AccountId = accountId;
        }

        public CreateUserBuilder WithName(string name) {
            Fields["user[name]"] = name;
            return this;
        }

        public CreateUserBuilder WithShortName(string shortName) {
            Fields["user[short_name]"] = shortName;
            return this;
        }

        public CreateUserBuilder WithSortableName(string sortableName) {
            Fields["user[sortable_name]"] = sortableName;
            return this;
        }

        public CreateUserBuilder WithTimeZone(string timeZone) {
            Fields["user[time_zone]"] = timeZone;
            return this;
        }

        public CreateUserBuilder WithLocale(string locale) {
            Fields["user[locale]"] = locale;
            return this;
        }

        public CreateUserBuilder WithBirthDate(string date) {
            Fields["user[birthdate]"] = date;
            return this;
        }

        public CreateUserBuilder WithTermsOfUseAccepted(bool hasAcceptedTerms = true) {
            Fields["user[terms_of_use]"] = hasAcceptedTerms.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithSkipRegistration(bool skipRegistration = true) {
            Fields["user[skip_registration]"] = skipRegistration.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithUniqueId(string uniqueId) {
            Fields["pseudonym[unique_id]"] = uniqueId;
            return this;
        }

        public CreateUserBuilder WithPassword(string password) {
            Fields["pseudonym[password]"] = password;
            return this;
        }

        public CreateUserBuilder WithSisUserId(string sis) {
            Fields["pseudonym[sis_user_id]"] = sis;
            return this;
        }

        public CreateUserBuilder WithIntegrationId(string integrationId) {
            Fields["pseudonym[integration_id]"] = integrationId;
            return this;
        }

        public CreateUserBuilder WithSendConfirmation(bool shouldSendConfirmation = true) {
            Fields["pseudonym[send_confirmation]"] = shouldSendConfirmation.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithForceSelfRegistration(bool shouldForceSelfRegistration = true) {
            Fields["pseudonym[force_self_registration]"] = shouldForceSelfRegistration.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithAuthProviderId(string authProviderId) {
            Fields["pseudonym[authentication_provider_id]"] = authProviderId;
            return this;
        }

        public CreateUserBuilder WithCommunicationChannelType(string type) {
            Fields["communication_channel[type]"] = type;
            return this;
        }

        public CreateUserBuilder WithCommunicationChannelAddress(string address) {
            Fields["communication_channel[address]"] = address;
            return this;
        }

        public CreateUserBuilder WithCommunicationChannelConfirmationUrl(bool returnUrl = true) {
            Fields["communication_channel[confirmation_url]"] = returnUrl.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithSkipCommunicationChannelConfirmation(bool skip = true) {
            Fields["communication_channel[skip_confirmation]"] = skip.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithForceValidations(bool force = true) {
            Fields["force_validations"] = force.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithSisReactivation(bool enable = true) {
            Fields["enable_sis_reactivation"] = enable.ToString().ToLower();
            return this;
        }

        public CreateUserBuilder WithDestination(string url) {
            Fields["destination"] = url;
            return this;
        }

        public Task<User> Post() {
            return _api.CreateUser(this);
        }
    }
}