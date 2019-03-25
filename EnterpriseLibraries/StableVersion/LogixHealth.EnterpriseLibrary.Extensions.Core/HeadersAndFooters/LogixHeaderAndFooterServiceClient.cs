namespace LogixHealth.EnterpriseLibrary.Extensions.HeadersAndFooters
{
    [System.ServiceModel.ServiceContractAttribute]
    public interface IAppUser
    {
        [System.ServiceModel.OperationContractAttribute]
        LogixHealth.Connect.BusinessEntities.UserHeader GetUserHeader(System.Guid userId, string organizationId);

        [System.ServiceModel.OperationContractAttribute]
        LogixHealth.Connect.BusinessEntities.UserHeaderInfo GetUserHeaderForCurrentArea(System.Guid userId, string currentArea, string helpId, string timeOut, string organizationId);

        [System.ServiceModel.OperationContractAttribute]
        LogixHealth.Connect.BusinessEntities.UserSubHeaderInfo GetUserSubHeaderForCurrentArea(System.Guid userId, int orgainzationId, string currentArea, string currentTab, string roleCode, string redirectFrom, string userRoleWithDepartmentCode);

        [System.ServiceModel.OperationContractAttribute]
        LogixHealth.Connect.BusinessEntities.ApplicationFooter GetUserFooterCurrentArea(string currentArea);
    }

    public class LogixHeaderAndFooterServiceClient : AppServices.Gateway.LogixServiceGateway<IAppUser>
    {
        public LogixHeaderAndFooterServiceClient(string binding, string serviceEndpoint) : base(binding, serviceEndpoint)
        {
        }

        public Connect.BusinessEntities.UserHeader GetUserHeader(System.Guid userId, string organizationId)
        {
            return Gateway.GetUserHeader(userId, organizationId);
        }

        public Connect.BusinessEntities.UserHeaderInfo GetUserHeaderForCurrentArea(System.Guid userId, string currentArea, string helpId, string timeOut, string organizationId)
        {
            return Gateway.GetUserHeaderForCurrentArea(userId, currentArea, helpId, timeOut, organizationId);
        }

        public Connect.BusinessEntities.UserSubHeaderInfo GetUserSubHeaderForCurrentArea(System.Guid userId, string currentArea, int organizationId, string currentTab, string roleCode, string redirectFrom, string userRoleWithDepartmentCode)
        {
            return Gateway.GetUserSubHeaderForCurrentArea(userId, organizationId, currentArea, currentTab, roleCode, redirectFrom, userRoleWithDepartmentCode);
        }

        public Connect.BusinessEntities.ApplicationFooter GetUserFooterCurrentArea(string currentArea)
        {
            return Gateway.GetUserFooterCurrentArea(currentArea);
        }
    }
}

namespace LogixHealth.Connect.BusinessEntities
{
    [System.Runtime.Serialization.DataContractAttribute]
    public class UserHeader
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string DisplayName { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HelpId { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public UserApplication[] UserApplications { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute]
    public class UserApplication
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Url { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool OpenInNewWindow { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute]
    public class UserHeaderInfo
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int OrganizationId { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public OrganizationHeader[] UserOrganizations { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OutputHtmlHeader { get; set; }

        public string ApplicationName { get; set; }

        public string DisplayName { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute]
    public class OrganizationHeader
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Id { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name { get; set; }
    }

    [System.Runtime.Serialization.DataContractAttribute]
    public class UserSubHeaderInfo
    {
    }

    [System.Runtime.Serialization.DataContractAttribute]
    public class ApplicationFooter
    {
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ApplicationFooterPart1 { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CopyrightYear { get; set; }
    }
}
