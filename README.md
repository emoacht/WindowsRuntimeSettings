Windows Runtime Settings
========================

A library for Windows Runtime app to save/load settings of various types with encryption if necessary. It utilizes LocalSettings/RoamingSettings, LocalFolder/RoamingFolder and PasswordVault as backing stores of properties for settings.

##Requirements

 * Some parts use features of C# 6.0 and so require Visual Studio 2015.

##Usage

| Attribute           | Description                                                                                                                                               |
|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------------------|
| RoamingAttribute    | Property values are saved in RoamingSettings or RoamingFolder (if CryptFileAttribute is also attached). Otherwise, the values are saved in LocalSettings. |
| CryptVaultAttribute | Property values are saved in PasswordVault. The number of items is limited to 10 at the maximum.                                                          |
| CryptFileAttribute  | Property values are encrypted and saved in a file in LocalFolder or RoamingFolder (if RoamingAttribute is also attached).                                 |

##License

 - MIT License
