using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace WindowsRuntimeSettings.Accessor
{
	/// <summary>
	/// Save/Load settings in PasswordVault (10 items at the maximum).
	/// </summary>
	internal class CryptVaultRootAccessor : IRootAccessor
	{
		private readonly string _resourceName;
		private readonly IStringConverter _converter;
		private readonly PasswordVault _vault = new PasswordVault();

		public CryptVaultRootAccessor(string resourceName)
			: this(resourceName, new StringConverter())
		{ }

		public CryptVaultRootAccessor(string resourceName, IStringConverter converter)
		{
			if (string.IsNullOrWhiteSpace(resourceName))
				throw new ArgumentNullException(nameof(resourceName));

			_resourceName = resourceName;
			_converter = converter;
		}

		#region Getter

		public T GetValue<T>(string propertyName, bool isRoaming) // Roaming will be ignored.
		{
			T propertyValue;
			TryGetValue(out propertyValue, propertyName, isRoaming);
			return propertyValue;
		}

		public T GetValue<T>(T defaultValue, string propertyName, bool isRoaming) // Roaming will be ignored.
		{
			T propertyValue;
			if (TryGetValue(out propertyValue, propertyName, isRoaming))
				return propertyValue;
			else
				return defaultValue;
		}

		public bool TryGetValue<T>(out T propertyValue, string propertyName, bool isRoaming) // Roaming will be ignored.
		{
			var credential = GetCredential(_vault, _resourceName, propertyName);
			if (credential != null)
			{
				credential.RetrievePassword();
				propertyValue = _converter.ConvertFromString<T>(credential.Password);
				return true;
			}
			else
			{
				propertyValue = default(T);
				return false;
			}
		}

		#endregion

		#region Setter

		public void SetValue<T>(T propertyValue, string propertyName, bool isRoaming) // Roaming will be ignored.
		{
			if (!EqualityComparer<T>.Default.Equals(propertyValue, default(T)))
			{
				var valueString = _converter.ConvertToString(propertyValue);

				// Create or overwrite a credential.
				var credential = new PasswordCredential(_resourceName, propertyName, valueString);
				_vault.Add(credential);
			}
			else
			{
				// Remove a credential.
				var credential = GetCredential(_vault, _resourceName, propertyName);
				if (credential != null)
				{
					_vault.Remove(credential);
				}
			}
		}

		#endregion

		#region Access

		private static PasswordCredential GetCredential(PasswordVault vault, string resourceName, string propertyName)
		{
			return vault.RetrieveAll()
				.Where(x => x.Resource == resourceName)
				.SingleOrDefault(x => x.UserName == propertyName);
		}

		#endregion
	}
}