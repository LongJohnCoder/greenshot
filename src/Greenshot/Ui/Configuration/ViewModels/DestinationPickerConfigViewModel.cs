﻿#region Greenshot GNU General Public License

// Greenshot - a free and open source screenshot tool
// Copyright (C) 2007-2018 Thomas Braun, Jens Klingen, Robin Krom
// 
// For more information see: http://getgreenshot.org/
// The Greenshot project is hosted on GitHub https://github.com/greenshot/greenshot
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 1 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using GongSolutions.Wpf.DragDrop;
using Greenshot.Addons;
using Greenshot.Addons.Addons;
using Greenshot.Addons.Core;
using Greenshot.Addons.Extensions;
using Greenshot.Configuration;

namespace Greenshot.Ui.Configuration.ViewModels
{
    [Export(typeof(IConfigScreen))]
    public sealed class DestinationPickerConfigViewModel : SimpleConfigScreen
    {
        /// <summary>
        ///     Here all disposables are registered, so we can clean the up
        /// </summary>
        private CompositeDisposable _disposables;

        [Import]
        public ICoreConfiguration CoreConfiguration { get; set; }

        [Import]
        public IConfigTranslations ConfigTranslations { get; set; }

        [ImportMany(AllowRecomposition = true)]
        private IEnumerable<Lazy<IDestination, IDestinationMetadata>> AllDestinations = null;

        [Import]
        public IGreenshotLanguage GreenshotLanguage { get; set; }

        public override void Initialize(IConfig config)
        {
            // Prepare disposables
            _disposables?.Dispose();
            _disposables = new CompositeDisposable();

            // Place this under the Ui parent
            ParentId = nameof(ConfigIds.Destinations);

            // Make sure Commit/Rollback is called on the IUiConfiguration
            config.Register(CoreConfiguration);

            // automatically update the DisplayName
            var greenshotLanguageBinding = GreenshotLanguage.CreateDisplayNameBinding(this, nameof(IGreenshotLanguage.SettingsDestinationPicker));

            // Make sure the greenshotLanguageBinding is disposed when this is no longer active
            _disposables.Add(greenshotLanguageBinding);

            UsedDestinations.Clear();
            if (CoreConfiguration.PickerDestinations.Any())
            {
                foreach (var outputDestination in CoreConfiguration.PickerDestinations)
                {
                    var pickerDestination = AllDestinations
                        .Where(destination => !"Picker".Equals(destination.Metadata.Designation))
                        .Where(destination => destination.Value.IsActive)
                        .Where(destination => outputDestination == destination.Value.Designation)
                        .Select(d => d.Value).FirstOrDefault();

                    if (pickerDestination != null)
                    {
                        UsedDestinations.Add(pickerDestination);
                    }
                }
            }
            else
            {
                foreach (var pickerDestination in AllDestinations
                    .Where(destination => !"Picker".Equals(destination.Metadata.Designation))
                    .Where(destination => destination.Value.IsActive)
                    .OrderBy(destination => destination.Metadata.Priority)
                    .ThenBy(destination => destination.Value.Description)
                    .Select(d => d.Value))
                {
                    UsedDestinations.Add(pickerDestination);
                }
            }
            AvailableDestinations.Clear();
            foreach (var destination in AllDestinations
                .Where(destination => !"Picker".Equals(destination.Metadata.Designation))
                .Where(destination => destination.Value.IsActive)
                .Where(destination => UsedDestinations.All(pickerDestination => pickerDestination.Designation != destination.Value.Designation))
                .OrderBy(destination => destination.Metadata.Priority).ThenBy(destination => destination.Value.Description)
                .Select(d => d.Value))
            {
                AvailableDestinations.Add(destination);
            }
            base.Initialize(config);
        }

        public override void Commit()
        {
            CoreConfiguration.PickerDestinations = UsedDestinations.Select(d => d.Designation).ToList();
            base.Commit();
        }

        protected override void OnDeactivate(bool close)
        {
            _disposables.Dispose();
            base.OnDeactivate(close);
        }

        public ObservableCollection<IDestination> AvailableDestinations { get; } = new ObservableCollection<IDestination>();

        public ObservableCollection<IDestination> UsedDestinations { get; } = new ObservableCollection<IDestination>();
        
    }
}
