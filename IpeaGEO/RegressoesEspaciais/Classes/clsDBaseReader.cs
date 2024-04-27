using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace IpeaGeo.RegressoesEspaciais
{
    public class clsDBaseReader
    {
        public string[] ColumnNames
        {
            get
            {
                int n_cols = (int)this.ColumnCount;
                string[] vars = new string[n_cols];
                for (int i=0; i<n_cols; i++)
                    vars[i] = _header.Columns[i].ColumnName;
                return vars;
            }
        }

        public int[] ColumnLengths
        {
            get
            {
                int n_cols = (int)this.ColumnCount;
                int[] vars = new int[n_cols];
                for (int i = 0; i < n_cols; i++)
                    vars[i] = _header.Columns[i].Length;
                return vars;
            }
        }

        public Type[] ColumnTypes
        {
            get
            {
                int n_cols = (int)this.ColumnCount;
                Type[] vars = new Type[n_cols];
                for (int i = 0; i < n_cols; i++)
                    vars[i] = _header.Columns[i].DataType;
                return vars;
            }
        }

        private clsDBaseHeader _header;
		private string _filename;
		private FileStream _dbaseFileStream;
		private BinaryReader _dbaseReader;
        private bool _headerIsParsed;
        private bool _isOpen;
        private Encoding _encoding;
        private bool _isDisposed = false;

        /// <summary>
        /// Creates a new instance of a <see cref="DBaseReader"/> from the 
        /// <paramref name="filename"> specified path</paramref>.
        /// </summary>
        /// <param name="filename">The path of the DBF file to read</param>
        /// <exception cref="FileNotFoundException"><paramref name="filename">The passed 
        /// filename</paramref> doesn't exist</exception>
		public clsDBaseReader(string filename)
		{
			if (!File.Exists(filename))
				throw new FileNotFoundException(String.Format("Could not find file \"{0}\"", filename));
			_filename = filename;
			_headerIsParsed = false;
		}

        ~clsDBaseReader()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets a value which indicates if the reader is open: true if it is, false otherwise.
        /// </summary>
		public bool IsOpen
		{
			get { return _isOpen; }
		}
	
        /// <summary>
        /// Opens the <see cref="DbaseReader"/> on the file specified on creation.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Thrown when the method is called 
        /// and object has been disposed</exception>
        public void Open()
        {
            Open(false);
        }

		public void Open(bool exclusive)
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Attempt to access a disposed DbaseReader object");
            
            if(exclusive)
			    _dbaseFileStream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            else
                _dbaseFileStream = new FileStream(_filename, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);

			_dbaseReader = new BinaryReader(_dbaseFileStream);
			_isOpen = true;

            if (!_headerIsParsed) //Don't read the header if it's already parsed
            {
                _header = clsDBaseHeader.ParseDbfHeader(_dbaseReader);
                //_baseTable = clsDBaseSchema.GetFeatureTableForFields(_header.Columns);
                _headerIsParsed = true;
            }
		}

        /// <summary>
        /// Closes the xBase file.
        /// </summary>
        /// <seealso cref="IsOpen"/>
        /// <seealso cref="Open"/>
        /// <seealso cref="Dispose" />
        /// <exception cref="ObjectDisposedException">Thrown when the method is called and
        /// object has been disposed</exception>
		public void Close()
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Attempt to access a disposed DbaseReader object");

            if(_dbaseReader != null)
			    _dbaseReader.Close();

            if(_dbaseFileStream != null)
			    _dbaseFileStream.Close();
            
			_isOpen = false;
		}

        /// <summary>
        /// Closes all files and disposes of all resources.
        /// </summary>
        /// <seealso cref="Close"/>
		public void Dispose()
		{
            Dispose(true);
            GC.SuppressFinalize(this);
		}

        protected virtual void Dispose(bool isDisposing)
        {
            if (_isDisposed) // Idempotency
                return;

            if (isDisposing) // Do deterministic finalization of managed resources
            {
                if (_isOpen)
                    this.Close();

                _dbaseReader = null;
                _dbaseFileStream = null;
                //_baseTable.Dispose();
                //_baseTable = null;
            }

            // Clean up any unmanaged resources
            IsDisposed = true;
        }

        /// <summary>
        /// Gets a value which indicates if this object is disposed: true if it is, false otherwise
        /// </summary>
        /// <seealso cref="Dispose"/>
        public bool IsDisposed
        {
            get { return _isDisposed; }
            private set { _isDisposed = value; }
        }

        public UInt32 ColumnCount
        {
            get { return (uint)_header.Columns.Length; }
        }

        /// <summary>
        /// Number of records in the DBF file.
        /// </summary>
        public UInt32 RecordCount
        {
            get { return _header.RecordCount; }
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Text.Encoding"/> used for parsing strings 
        /// from the DBase DBF file.
        /// </summary>
        /// <remarks>
        /// If the encoding type isn't set, the dbase driver will try to determine 
        /// the correct <see cref="System.Text.Encoding"/>.
        /// </remarks>
        /// <exception cref="ObjectDisposedException">Thrown when the property is 
        /// fetched and/or set and object has been disposed</exception>
        public System.Text.Encoding Encoding
        {
            get
            {
                if (_isDisposed)
                    throw new ObjectDisposedException("Attempt to access a disposed DbaseReader object");
                return _encoding;
            }
            set
            {
                if (_isDisposed)
                    throw new ObjectDisposedException("Attempt to access a disposed DbaseReader object");
                _encoding = value;
            }
        }

        //private NumberFormatInfo NumberFormat_usado = new CultureInfo("en-US", false).NumberFormat;
        private NumberFormatInfo NumberFormat_usado = new CultureInfo("pt-BR", false).NumberFormat;

        private NumberStyles m_style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands
                                     | NumberStyles.AllowCurrencySymbol | NumberStyles.Number | NumberStyles.AllowExponent;
        private CultureInfo m_culture = CultureInfo.GetCultureInfo("en-US");


    }
}
