	// Method 1.
	// Method 1.
	// Method 1.
	// Method 1.
	// Method 1.
	// Method 1.

		public bool IsNumeric(string Nombre)
		{
			int i = 0;
			int nb = 0;
			bool ok = false;
			char[] tabNombre;
			char[] unNb;

			tabNombre = Nombre.ToCharArray(0, Nombre.Length);

			for (i=0; i < Nombre.Length; i++)
			{
				ok = false;
				while ((nb < 10) && (ok == false))
				{
					unNb = Convert.ToString(nb).ToCharArray(0, 1);
					if (tabNombre[i] == unNb[0])
					{
						ok = true;
						nb = 0;
					}
					else
					{
						if ((i == 0) && (tabNombre[i] == '-'))
						{
							ok = true;
							nb = 0;
						}
						else
						{
							ok = false;
							nb++;
						}
					}
				}
			}
			return ok;
		}


		// Method 2.
		// Method 2.
		// Method 2.
		// Method 2.
		// Method 2.
		// Method 2.
		// Method 2.

private static Regex _isNumber = new Regex(@"^\d+$");

public static bool IsInteger(string theValue)
{
Match m = _isNumber.Match(theValue);
return m.Success;
} //IsInteger