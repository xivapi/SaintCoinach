using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.MefExtensions;

namespace Thaliak {
    class ThaliakBootstrapper : MefBootstrapper {
        // TODO: Get these proper
        private string _GamePath = @"C:\Program Files (x86)\SquareEnix\FINAL FANTASY XIV - A Realm Reborn";
        private SaintCoinach.Ex.Language _Language = SaintCoinach.Ex.Language.English;

        private SaintCoinach.ARealmReversed _Arr;
        private Controllers.NavigationController _NavigationController;

        #region Initialize
        protected override void InitializeShell() {
            base.InitializeShell();

            _NavigationController = Container.GetExportedValue<Controllers.NavigationController>();

            Application.Current.MainWindow = (Shell)this.Shell;
            Application.Current.MainWindow.Show();
        }
        #endregion

        #region Configuration
        protected override void ConfigureAggregateCatalog() {
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(ThaliakBootstrapper).Assembly));
            this.AggregateCatalog.Catalogs.Add(new AssemblyCatalog(typeof(RegionNames).Assembly));

            base.ConfigureAggregateCatalog();
        }

        protected override Microsoft.Practices.Prism.Modularity.IModuleCatalog CreateModuleCatalog() {
            if (System.IO.Directory.Exists(@".\Modules"))
                return new DirectoryModuleCatalog() { ModulePath = @".\Modules" };
            return base.CreateModuleCatalog();
        }

        protected override void ConfigureContainer() {
            base.ConfigureContainer();
        }
        protected override void RegisterBootstrapperProvidedTypes() {
            base.RegisterBootstrapperProvidedTypes();

            _Arr = new SaintCoinach.ARealmReversed(_GamePath, _Language);
            this.Container.ComposeExportedValue<SaintCoinach.ARealmReversed>(_Arr);
            this.Container.ComposeExportedValue<SaintCoinach.IO.PackCollection>(_Arr.Packs);
            this.Container.ComposeExportedValue<SaintCoinach.Ex.ExCollection>(_Arr.GameData);
            this.Container.ComposeExportedValue<SaintCoinach.Ex.Relational.RelationalExCollection>(_Arr.GameData);
            this.Container.ComposeExportedValue<SaintCoinach.Xiv.XivCollection>(_Arr.GameData);

            var nanaBear = _Arr.GameData.GetSheet<SaintCoinach.Xiv.Item>()[8200];
            var nanaBearName = nanaBear.Name;
        }

        protected override System.Windows.DependencyObject CreateShell() {
            return this.Container.GetExportedValue<Shell>();
        }
        protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors() {
            var factory = base.ConfigureDefaultRegionBehaviors();
            {
                var t = typeof(Behaviors.AutoPopulateExportedViewsBehavior);
                factory.AddIfMissing(t.Name, t);
            }/*
            {
                var t = typeof(Behaviors.RegionAwareBehaviour);
                factory.AddIfMissing(t.Name, t);
            }*/

            return factory;
        }

        protected override Microsoft.Practices.Prism.Regions.RegionAdapterMappings ConfigureRegionAdapterMappings() {
            var mappings = base.ConfigureRegionAdapterMappings();


            return mappings;
        }

        protected override ILoggerFacade CreateLogger() {
            return new EnterpriseLoggingAdapter();
        }
        #endregion
    }
}
