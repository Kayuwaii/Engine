using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.CoreTests.IO
{
	public class CsvIterator : IDisposable
	{
		private string _csvPath { get; init; }
		private FileStream reader { get; set; }
		public int TotalLines { get; set; } = 0;
		private int currentLine = 0;
		private bool disposedValue;

		public virtual bool HasNext => currentLine < TotalLines;

		public CsvIterator(string csvPath)
		{
			_csvPath = csvPath;
			OpenFile();
		}

		private void OpenFile()
		{
			reader = new FileStream(_csvPath, FileMode.Open, FileAccess.Read);

			byte[] buffer = new byte[1024];
			int bytesRead;
			while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
			{
				for (int i = 0; i < bytesRead; i++)
				{
					if (buffer[i] == '\n')
					{
						TotalLines++;
					}
				}
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: eliminar el estado administrado (objetos administrados)
					reader.Close();
					reader.Dispose();
				}

				// TODO: liberar los recursos no administrados (objetos no administrados) y reemplazar el finalizador
				// TODO: establecer los campos grandes como NULL
				disposedValue = true;
			}
		}

		// // TODO: reemplazar el finalizador solo si "Dispose(bool disposing)" tiene código para liberar los recursos no administrados
		// ~CsvIterator()
		// {
		//     // No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
		//     Dispose(disposing: false);
		// }

		public void Dispose()
		{
			// No cambie este código. Coloque el código de limpieza en el método "Dispose(bool disposing)".
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
