﻿using System;
using System.Security.Cryptography;

namespace ConsoleApp1
{
	class Program
	{
		const int iterate = 10000;

		static void Main(string[] args)
		{
			Random random = new Random();
			byte[] salt = new byte[16];
			random.NextBytes(salt);

			GeneratePasswordHashUsingSalt("password", salt);
			GeneratePasswordHashUsingSalt_Optimized("password", salt);
		}

		public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
		{
			var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);
			byte[] hash = pbkdf2.GetBytes(20);
			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);
			var passwordHash = Convert.ToBase64String(hashBytes);
			return passwordHash;
		}

		public static string GeneratePasswordHashUsingSalt_Optimized(string password, byte[] salt)
		{
			PBKDF2 pbkdf2 = new PBKDF2(password, salt, iterate);
			byte[] hash = pbkdf2.GetBytes(20);
			byte[] hashBytes = new byte[36];
			Array.Copy(salt, 0, hashBytes, 0, 16);
			Array.Copy(hash, 0, hashBytes, 16, 20);
			var passwordHash = Convert.ToBase64String(hashBytes);
			return passwordHash;
		}
	}
}
