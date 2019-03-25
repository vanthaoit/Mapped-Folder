namespace LogixHealth.EnterpriseLibrary.Extensions.Configurations
{
    using System.Configuration;

    /* 
        <LogixConfigurations>
            <ConnectAuthentication TokenProvider="Token Provider" RelayingParty="Relaying Party" Thumbprint="Thumbprint" />

            <HeaderAndFooter Address="URL Address" Binding="Address Binding" UseNewConnectHeader="Use New Connect Header" />

            <LogixLogger ApplicationName="Application Name" Address="URL Address" Binding="Address Binding" BaseLocationOfLogFile="C:\\LogixLogs\\" UseMessageQueue="NO" QueueLocation="My Private Queue" />

            <ServiceEndpoints>
                <ServiceEndpoint Name="AppService" Address="URL Address" Binding="Address Binding" />
            </ServiceEndpoints>

            <ApiEndpoints>
                <ApiEndpoint Name="ApplicationAPI" Address="URL Address" />
            </ApiEndpoints>        
        </LogixConfigurations>
    */

    public interface ILogixConfiguration
    {
        [ConfigurationProperty(name: "ConnectAuthentication")]
        ConnectAuthentication ConnectAuthenticationSettings { get; set; }

        [ConfigurationProperty(name: "HeaderAndFooter")]
        HeaderAndFooter HeaderAndFooterSettings { get; set; }

        [ConfigurationProperty(name: "LogixLogger")]
        LogixLogger LogixLoggerSettings { get; set; }

        [ConfigurationProperty(name: "ServiceEndpoints")]
        ServiceEndpointCollection ServiceEndpoints { get; }

        [ConfigurationProperty(name: "ApiEndpoints")]
        ApiEndpointCollection ApiEndpoints { get; }
    }
}

