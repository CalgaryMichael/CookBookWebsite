using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CookBookWebSite.Service.SQL {
	public static class Scripts {
		public static string SqlRoot { get; }

		public static string CreateDatabaseSql { get; }
		public static string CreateTablesSql { get; }
		public static string InsertDummyDataSql { get; }

		public static Dictionary<string, string> ScriptDictionary { get; }


		static Scripts() {
			SqlRoot = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent?.FullName + "/CookBookWebSite/Service/SQL";
			
			ScriptDictionary = Directory.GetFiles(SqlRoot, "*.sql").ToDictionary(Path.GetFileNameWithoutExtension, File.ReadAllText, StringComparer.OrdinalIgnoreCase);
			Debug.WriteLine(ScriptDictionary);
			CreateDatabaseSql = ScriptDictionary["CreateDatabase"];
			CreateTablesSql = ScriptDictionary["CreateTables"];
			InsertDummyDataSql = ScriptDictionary["InsertDummyData"];
		}

	}
}