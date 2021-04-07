﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.208
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rawr.Server
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="Rawr")]
	public partial class RawrDBDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertCharacterXML(CharacterXML instance);
    partial void UpdateCharacterXML(CharacterXML instance);
    partial void DeleteCharacterXML(CharacterXML instance);
    partial void InsertServerCharacterXML(ServerCharacterXML instance);
    partial void UpdateServerCharacterXML(ServerCharacterXML instance);
    partial void DeleteServerCharacterXML(ServerCharacterXML instance);
    #endregion
		
		public RawrDBDataContext() : 
				base(global::System.Configuration.ConfigurationManager.ConnectionStrings["RawrConnectionString"].ConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public RawrDBDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RawrDBDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RawrDBDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public RawrDBDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<CharacterXML> CharacterXMLs
		{
			get
			{
				return this.GetTable<CharacterXML>();
			}
		}
		
		public System.Data.Linq.Table<ServerCharacterXML> ServerCharacterXMLs
		{
			get
			{
				return this.GetTable<ServerCharacterXML>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="rawrserver.CharacterXML")]
	public partial class CharacterXML : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _CharacterName;
		
		private string _Realm;
		
		private string _Region;
		
		private System.DateTime _LastRefreshed;
		
		private string _XML;
		
		private string _CurrentModel;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCharacterNameChanging(string value);
    partial void OnCharacterNameChanged();
    partial void OnRealmChanging(string value);
    partial void OnRealmChanged();
    partial void OnRegionChanging(string value);
    partial void OnRegionChanged();
    partial void OnLastRefreshedChanging(System.DateTime value);
    partial void OnLastRefreshedChanged();
    partial void OnXMLChanging(string value);
    partial void OnXMLChanged();
    partial void OnCurrentModelChanging(string value);
    partial void OnCurrentModelChanged();
    #endregion
		
		public CharacterXML()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CharacterName", DbType="NVarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string CharacterName
		{
			get
			{
				return this._CharacterName;
			}
			set
			{
				if ((this._CharacterName != value))
				{
					this.OnCharacterNameChanging(value);
					this.SendPropertyChanging();
					this._CharacterName = value;
					this.SendPropertyChanged("CharacterName");
					this.OnCharacterNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Realm", DbType="NVarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Realm
		{
			get
			{
				return this._Realm;
			}
			set
			{
				if ((this._Realm != value))
				{
					this.OnRealmChanging(value);
					this.SendPropertyChanging();
					this._Realm = value;
					this.SendPropertyChanged("Realm");
					this.OnRealmChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Region", DbType="Char(2) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string Region
		{
			get
			{
				return this._Region;
			}
			set
			{
				if ((this._Region != value))
				{
					this.OnRegionChanging(value);
					this.SendPropertyChanging();
					this._Region = value;
					this.SendPropertyChanged("Region");
					this.OnRegionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastRefreshed", DbType="DateTime NOT NULL")]
		public System.DateTime LastRefreshed
		{
			get
			{
				return this._LastRefreshed;
			}
			set
			{
				if ((this._LastRefreshed != value))
				{
					this.OnLastRefreshedChanging(value);
					this.SendPropertyChanging();
					this._LastRefreshed = value;
					this.SendPropertyChanged("LastRefreshed");
					this.OnLastRefreshedChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_XML", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string XML
		{
			get
			{
				return this._XML;
			}
			set
			{
				if ((this._XML != value))
				{
					this.OnXMLChanging(value);
					this.SendPropertyChanging();
					this._XML = value;
					this.SendPropertyChanged("XML");
					this.OnXMLChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CurrentModel", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string CurrentModel
		{
			get
			{
				return this._CurrentModel;
			}
			set
			{
				if ((this._CurrentModel != value))
				{
					this.OnCurrentModelChanging(value);
					this.SendPropertyChanging();
					this._CurrentModel = value;
					this.SendPropertyChanged("CurrentModel");
					this.OnCurrentModelChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="rawrserver.ServerCharacterXML")]
	public partial class ServerCharacterXML : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _CharacterName;
		
		private string _SavePassword;
		
		private string _XML;
		
		private System.DateTime _LastModified;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCharacterNameChanging(string value);
    partial void OnCharacterNameChanged();
    partial void OnSavePasswordChanging(string value);
    partial void OnSavePasswordChanged();
    partial void OnXMLChanging(string value);
    partial void OnXMLChanged();
    partial void OnLastModifiedChanging(System.DateTime value);
    partial void OnLastModifiedChanged();
    #endregion
		
		public ServerCharacterXML()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CharacterName", DbType="NVarChar(50) NOT NULL", CanBeNull=false, IsPrimaryKey=true)]
		public string CharacterName
		{
			get
			{
				return this._CharacterName;
			}
			set
			{
				if ((this._CharacterName != value))
				{
					this.OnCharacterNameChanging(value);
					this.SendPropertyChanging();
					this._CharacterName = value;
					this.SendPropertyChanged("CharacterName");
					this.OnCharacterNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SavePassword", DbType="NVarChar(50)")]
		public string SavePassword
		{
			get
			{
				return this._SavePassword;
			}
			set
			{
				if ((this._SavePassword != value))
				{
					this.OnSavePasswordChanging(value);
					this.SendPropertyChanging();
					this._SavePassword = value;
					this.SendPropertyChanged("SavePassword");
					this.OnSavePasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_XML", DbType="NText NOT NULL", CanBeNull=false, UpdateCheck=UpdateCheck.Never)]
		public string XML
		{
			get
			{
				return this._XML;
			}
			set
			{
				if ((this._XML != value))
				{
					this.OnXMLChanging(value);
					this.SendPropertyChanging();
					this._XML = value;
					this.SendPropertyChanged("XML");
					this.OnXMLChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LastModified", DbType="DateTime NOT NULL")]
		public System.DateTime LastModified
		{
			get
			{
				return this._LastModified;
			}
			set
			{
				if ((this._LastModified != value))
				{
					this.OnLastModifiedChanging(value);
					this.SendPropertyChanging();
					this._LastModified = value;
					this.SendPropertyChanged("LastModified");
					this.OnLastModifiedChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591