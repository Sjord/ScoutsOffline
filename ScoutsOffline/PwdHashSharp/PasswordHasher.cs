// PasswordHasher.cs
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
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace PwdHashSharp
{
	/// <summary>
	/// A class to create strong passwords that are always reproducible given
	/// a URI from the original domain, and the passphrase.  The algorithm
	/// uses a HMAC MD5 hash, along with a pseudo-random function used to 
	/// assure password strength, and obfuscate the process.
	/// </summary>
	public class PasswordHasher
	{

#region Public Constructors
		/// <summary>
		/// Create a new PasswordHasher.  Initialize with the full domain that
		/// should be hashed.
		/// </summary>
		/// <param name="domain">The <see cref="System.String"/> domain component
		/// of an original URI</param>
		/// <param name="passphrase">The "master password" that is used for
		/// hashing with.</param>
		public PasswordHasher(string domain, string passphrase)
		{
			Debug.Assert(null != domain);
			Debug.Assert(null != passphrase);
			Domain = domain;
			Passphrase = passphrase;
		}
#endregion
		
#region Public Methods		
		/// <summary>
		/// Create a strong password from the given URI and passphrase
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> representing the generated password
		/// </returns>
		public string GetHashedPassword()
		{
			// Get initial hash
			string hash = B64HmacMd5(Passphrase, Domain);
			
			// Apply constraints
			int size = Passphrase.Length + PwdHashSharp.PasswordPrefix.Length;
			bool nonalphanumeric = new Regex("\\W").IsMatch(Passphrase);
			string result = ApplyConstraints(hash, size, nonalphanumeric);
			
			return result;
		}
#endregion
		
#region Protected Methods		
		/// <summary>
		/// Create a base-64 encoded HMAC MD5 hash from a key and data string
		/// </summary>
		/// <param name="password">
		/// A <see cref="System.String"/> representing the inital password.
		/// This will be used as the "key" for MD5 encryption
		/// </param>
		/// <param name="realm">
		/// A <see cref="System.String"/> representing the initial password.
		/// This will be used as the "data" for MD5 encryption
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/> representing the base-64 encoded hash
		/// function
		/// </returns>
		protected string B64HmacMd5(string password, string realm) {

			Debug.WriteLine("B64HmacMd5:");
			Debug.Indent();
			Debug.WriteLine("password:\t" + password);
			Debug.WriteLine("realm:\t\t" + realm);

			// Put data in byte arrays to use for MD5
			byte[] key = System.Text.Encoding.ASCII.GetBytes(password);
			byte[] data = System.Text.Encoding.ASCII.GetBytes(realm);
			
			// Create our hash
			HMACMD5 hmac = new HMACMD5(key);
			byte[] hashb = hmac.ComputeHash(data);
			
			// Store it as a base-64 string
			string hash = Convert.ToBase64String(hashb);
			
			// Remove trailing "=="'s, which are added for strict adherence
			// to MD5 RFC.
			Regex reg = new Regex("=+$");
			string newhash = reg.Replace(hash, "");

			Debug.WriteLine("hash:\t\t" + newhash);
			Debug.Unindent();

			return newhash;
		}
		
		/// <summary>
		/// Augment the generated password to adhere to some constraints.
		/// Specifically, it should have at least one upper-case, one lower-case
		/// and one numeral character.  Also, if specified, it should contain
		/// at least one symbol.  Otherwise, it shouldn't contain any.  Then,
		/// trim it to be the specified length, and add some pseudo-entropy to
		/// make it harder to guess.
		/// </summary>
		/// <param name="hash">
		/// A <see cref="System.String"/> representing the initial password hash
		/// </param>
		/// <param name="size">
		/// A <see cref="System.Int32"/> containing the final size of the
		/// password
		/// </param>
		/// <param name="nonalphanumeric">
		/// A <see cref="System.Boolean"/> determining whether or not the final
		/// password should contain symbols
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/> containing the final string, with
		/// constraints applied.
		/// </returns>
		protected string ApplyConstraints(string hash, int size, bool nonalphanumeric)
		{
			Debug.WriteLine("ApplyConstraints:");
			Debug.Indent();

			int startingSize = size - 4; // Leave room for extra characters
			string result = hash.Substring(0, startingSize);
			extras = new Queue<char>(hash.Substring(startingSize).ToCharArray());
			Regex matchSymbol = new Regex("\\W");

			Debug.WriteLine("startingSize:\t" + startingSize);
			Debug.WriteLine("result (start):\t" + result);
			Debug.WriteLine("extras (start):\t" + new string(extras.ToArray()));
			Debug.WriteLine("nonalphanumeric:\t" + nonalphanumeric);
			
			// Add the extra characters
			Debug.Write("A-Z:\t\t\t");
			result += (new Regex("[A-Z]").IsMatch(result) ? nextExtraChar()
				                                          : nextBetween('A', 26));
			Debug.WriteLine(result);
			
			Debug.Write("a-z:\t\t\t");
			result += (new Regex("[a-z]").IsMatch(result) ? nextExtraChar()
				                                          : nextBetween('a', 26));
			Debug.WriteLine(result);
			
			Debug.Write("0-9:\t\t\t");
			result += (new Regex("[0-9]").IsMatch(result) ? nextExtraChar()
				                                          : nextBetween('0', 10));
			Debug.WriteLine(result);

			Debug.Write("\\W:\t\t\t");
			result += (new Regex("\\W").IsMatch(result) && nonalphanumeric ? 
			                                                nextExtraChar()
				                                          : '+');
			Debug.WriteLine(result);
			
			while(matchSymbol.IsMatch(result) && !nonalphanumeric) {
				Debug.Write("Replace '" + matchSymbol.Match(result) + "':\t\t");
				result = matchSymbol.Replace(result, nextBetween('A', 26).ToString(), 1);
				Debug.WriteLine(result);
			}

			// Rotate the result to make it harder to guess the inserted locations
			Debug.Write("Rotate ");
			char[] rotateArr = result.ToCharArray();
			rotateArr = rotate(rotateArr, nextExtra());
			result = new string(rotateArr);
			Debug.WriteLine(result);
			
			return result;
		}
#endregion
		
#region Private Helper Functions
		/* 
		 * These functions are used in the ApplyConstraints method.  They are
		 * provided as a convenience, and to make the code more readable.  They
		 * also exist in the original pwdhash javascript code.
		 */
		
		// Get the next extra character as an int if one exists-- otherwise 0
		private int nextExtra() {
			if (extras.Count != 0)
				return Convert.ToInt32(extras.Dequeue());
			else
				return 0;
		}
		
		// Get the next extra as a character
		private char nextExtraChar() {
			return Convert.ToChar(nextExtra());
		}
		
		// Rotate the character array a given number of times
		private char[] rotate(char[] arr, int amount) {
			Debug.Write(amount + "x:\t");
			Queue<char> q = new Queue<char>(arr);
			while (amount-- != 0)
				q.Enqueue(q.Dequeue());
			
			return q.ToArray();
		}
		
		// Return a integer within the given bounds
		private int between(int min, int interval, int offset) {
			return min + offset % interval;
		}
		
		// Get the next extra character within the given bounds
		private char nextBetween(char baseChar, int interval) {
			return Convert.ToChar(between(Convert.ToInt32(baseChar), interval, nextExtra()));
		}
#endregion
		
#region Data Members
		/// <summary>
		/// Domain component of a website to create a password first.  Should
		/// be pre-processed by DomainExtractor to remove extra characters.
		/// </summary>
		protected string Domain;
		
		/// <summary>
		/// User's master site password.  Hashed with the domain to create a 
		/// strong password.
		/// </summary>
		protected string Passphrase;
		
		// used by ApplyConstrains helper functions
		private Queue<char> extras;
#endregion	
	}
}
