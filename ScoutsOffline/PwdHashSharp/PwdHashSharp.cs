// PwdHashSharp.cs
// 
// Copyright (C) 2008 Scott Wegner
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Diagnostics;

namespace PwdHashSharp
{
	
	/// <summary>
	/// Wrapper class for the PwdHash Sharp application.  This takes care of
	/// calling other worker classes and generating the complete hashed password
	/// </summary>
	public partial class PwdHashSharp
	{
#region Public Methods
		/// <summary>
		/// Generate a hashed password based on the PwdHash algorithm
		/// </summary>
		/// <param name="siteAddress">
		/// A <see cref="System.String"/> representing the full URL of the site
		/// you would like to use the password for.
		/// </param>
		/// <param name="sitePassword">
		/// A <see cref="System.String"/> with the base password used for this
		/// site
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/> containing the unique, strong password
		/// generated using an MD5 hash and the PwdHash algorithm to ensure a
		/// strong and hard-to-guess password
		/// </returns>
		public string GenerateHashedPassword(string siteAddress, string sitePassword) {
			
			// Validate parameters
			if (siteAddress == null)
				throw new ArgumentNullException("siteAddress",
				                                "Please specify a valid Site Address");
			if (siteAddress.Equals(string.Empty))
				throw new ArgumentException("Please specify a valid Site Address",
				                            "siteAddress");
			if (sitePassword == null)
				throw new ArgumentNullException("sitePassword",
				                                "Please specify a valid Site Password");
			if (sitePassword.Equals(string.Empty))
				throw new ArgumentException("Please specify a valid Site Password",
				                            "sitePassword");
			
			// Get the base domain from the given URI
			string domain = DomainExtractor.ExtractDomain(siteAddress);
			
			// Make sure the new domain isn't empty
			if (domain.Equals(string.Empty))
				throw new ArgumentException("Please specify a valid Site Address",
				                            "siteAddress");
			
			// Then, use it along with the site password to apply the PwdHash 
			// algorithm.
			PasswordHasher hasher = new PasswordHasher(domain, sitePassword);
			string hashedPassword = hasher.GetHashedPassword();
			
			return hashedPassword;
		}
#endregion
		
#region Object Data
		/// <summary>
		/// A prefix used in the password generation process.  This is included
		/// only for compatibility with the original PwdHash plugin.  Note that
		/// other classes access this as well, so it must be public.
		/// </summary>
		public const string PasswordPrefix = "@@";	
#endregion
	}
}
