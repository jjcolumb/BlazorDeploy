using System;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlazorLinux.Blazor.Server.Services;

namespace BlazorLinux.Blazor.Server {
    public partial class BlazorLinuxBlazorApplication : BlazorApplication {
        #region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
        static BlazorLinuxBlazorApplication() {
            DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
            DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
            DevExpress.ExpressApp.BaseObjectSpace.ThrowExceptionForNotRegisteredEntityType = true;
        }
        #endregion
        public BlazorLinuxBlazorApplication() {
            InitializeComponent();
            SetupAuthentication();
            this.securityStrategyComplex1.RegisterXPOAdapterProviders();
        }
        private void SetupAuthentication() {
            this.authenticationMixed1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            this.authenticationMixed1.IsSupportChangePassword = true;
            this.authenticationMixed1.AddAuthenticationStandardProvider(typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser));
            this.authenticationMixed1.AddIdentityAuthenticationProvider(typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyUser));
        }
        protected override void OnSetupStarted() {
            base.OnSetupStarted();
            IConfiguration configuration = ServiceProvider.GetRequiredService<IConfiguration>();
            if(configuration.GetConnectionString("ConnectionString") != null) {
                ConnectionString = configuration.GetConnectionString("ConnectionString");
            }
#if EASYTEST
            if(configuration.GetConnectionString("EasyTestConnectionString") != null) {
                ConnectionString = configuration.GetConnectionString("EasyTestConnectionString");
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        }
        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args) {
            IXpoDataStoreProvider dataStoreProvider = GetDataStoreProvider(args.ConnectionString, args.Connection);
            args.ObjectSpaceProviders.Add(new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)Security, dataStoreProvider, true));
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
        }
        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, System.Data.IDbConnection connection) {
            XpoDataStoreProviderAccessor accessor = ServiceProvider.GetRequiredService<XpoDataStoreProviderAccessor>();
            lock(accessor) {
                if(accessor.DataStoreProvider == null) {
                    accessor.DataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, true);
                }
            }
            return accessor.DataStoreProvider;
        }
        private void BlazorLinuxBlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
            e.Updater.Update();
            e.Handled = true;
        }
    }
}
