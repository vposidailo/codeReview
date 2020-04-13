using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CodeReviewExample
{
	class ProductGenerator
	{
		public class Product
		{
			public string Id { get; set; }
			public string Name { get; set; }
			public string Category { get; set; }
			public string CreationalDate { get; set; }
		}

		public class Helper
		{
			public bool validate(Product a)
			{
				if (a != null)
				{
					if (!string.IsNullOrEmpty(a.Name))
					{
						if (a.Category == "Electronics")
						{
							return true;
						}
						else if (a.Category == "Sport")
						{
							return true;
						}
						else
						{
							return false;
						}
					}
					else
						return false;
				}
				else
					return false;
			}

			public void save(Product saveP)
			{
				string query = "INSERT INTO PRODUCT(id, name, Category, CD) Values (" + saveP.Id + "," + saveP.Name + "," + saveP.Category + "," + saveP.CreationalDate + ")";

#if DEBUG
				string connStr = "Server=(localhost);Database=Product_QA;User Id=john.boss@corp.com;Password=thisIsQAPassword;";
#endif
#if (RELEASE)
				string connStr = "Server=128.234.234.12/MSSQLSERVER;Database=Product_PROD;User Id=john.superboss@corp.com;Password=thisIsPROD;";
#endif
				SqlConnection conn = new SqlConnection(connStr);

				//open connection
				conn.Open();

				SqlCommand cmd = conn.CreateCommand();
				cmd.CommandText = query;
				cmd.ExecuteNonQuery();

				cmd.Dispose();
				conn.Close();
			}

			public bool Process(Product product)
			{
				try
				{
					var isValid = validate(product);

					if (!isValid)
					{
						return false;
					}

					save(product);

					return true;
				}
				catch(Exception ex)
				{
					return false;
				}
			}
		}

		public class ProductService
		{
			public Helper _h;

			public ProductService()
			{
				this._h = new Helper();
			}

			public void Save(Product a)
			{
				a.CreationalDate = DateTime.Now.ToString(@"dd/MM/yyyy");
				_h.Process(a);
			}
		}

		static void Main(string[] args)
		{
			try
			{
				List<Product> newProducts = new List<Product>()
				{
					new Product()
					{
						Id = "1",
						Name = "Ball",
						Category = "Sport"
					},
					new Product()
					{
						Id = "2",
						Name = "Laptop",
						Category = "Electronics"
					},
					new Product()
					{
						Id = "3",
						Name = "Tablet",
						Category = "Electronics"
					}
				};

				ProductService ps = new ProductService();

				foreach (var item in newProducts)
				{
					ps.Save(item);
				}
			}
			catch
			{

			}
		}
	}
}
