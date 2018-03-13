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

using System.ComponentModel.Composition;
using Dapplo.CaliburnMicro.Configuration;
using Dapplo.CaliburnMicro.Extensions;
using Greenshot.Configuration;

namespace Greenshot.Ui.Configuration.ViewModels
{
    /// <summary>
    /// This represents a node in the config
    /// </summary>
    [Export(typeof(IConfigScreen))]
    public sealed class DestinationsConfigNodeViewModel : ConfigNode
    {
        public IGreenshotLanguage GreenshotLanguage { get; }

        [ImportingConstructor]
        public DestinationsConfigNodeViewModel(IGreenshotLanguage greenshotLanguage)
        {
            GreenshotLanguage = greenshotLanguage;

            // automatically update the DisplayName
            GreenshotLanguage.CreateDisplayNameBinding(this, nameof(IGreenshotLanguage.SettingsDestination));

            // automatically update the DisplayName
            CanActivate = false;
            Id = nameof(ConfigIds.Destinations);
        }
    }
}
