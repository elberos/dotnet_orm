/**
 * This file is part of the Elberos package.
 *
 * (c) Ildar Bikmamatov <elberos@bayrell.org>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 */
 
using System;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace Elberos.Orm{
	
	public abstract class Connection {
		
		
		protected bool is_connected = false;
		
		
		/**
		 * Configures connection
		 */
		public abstract void Configure(IConfigurationSection section);
		
		
		/**
		 * Connect to DB
		 */
		public virtual void connect(){}
		
		
		/**
		 * Disconnect from DB
		 */
		public virtual void disconnect(){}
		
		
		/**
		 * Check DB connection
		 */
		public virtual bool connected(){
			return this.is_connected;
		}
		
		
		public virtual string escapeVar(string var){
			return var;
		}
		
		public virtual string escapeKey(string key){
			return key;
		}
		
		
		/**
		 * Create query builder
		 */
		public abstract QueryBuilder createQueryBuilder();
		
		
		/**
		 * Get last insert id
		 */
		public string lastInsertId(){
			return "";
		}
		
		
		/**
		 * DateTime string format
		 */
		public string getDateTimeFormatString(){
			return "Y-m-d H:i:s";
		}
		
		public string getDateFormatString(){
			return "Y-m-d";
		}
		
		public string getTimeFormatString(){
			return "H:i:s";
		}
		
		
		/**
		 * Convert functions
		 */
		public dynamic convertIntToDatabase(long a){
			return a;
		}
		public long convertIntFromDatabase(dynamic a){
			if (a == null) return 0;
			return (long)a;
		}
		public bool convertBooleanToDatabase(bool a){
			if (a) return true;
			return false;
		}
		public bool convertBooleanFromDatabase(dynamic a){
			if (a == null) return false;
			if (a) return true;
			return false;
		}
		public dynamic convertStringToDatabase(string s){
			return s;
		}
		public string convertStringFromDatabase(dynamic s){
			if (s == null) return "";
			return s.toString();
		}
		public dynamic convertObjectToDatabase(object a){
			return null;
			/*
			if (var === null) return null;
			if (gettype(var) != 'array') return null;
			return json_encode(var);
			*/
		}
		public object convertObjectFromDatabase(dynamic a){
			return null;
			/*
			if (var === null) return null;
			var = @json_decode(var, true);
			if (var === false) return null;
			if (gettype(var) != 'array') return null;
			return var;
			 */
		}
		
		
	}
	
}