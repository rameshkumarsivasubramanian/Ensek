using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ensek.DTO
{
    public class MeterReading
    {
        const char COLUMN_DELIMITER = ',';
        private readonly string _headerData;
        private readonly string _rowData;
        
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid {0}(numbers only).")]
        public string AccountId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public string MeterReadingDateTime { get; set; }
        [Required]
        [RegularExpression("^[0-9]{5}$", ErrorMessage = "Invalid {0}(numbers of 5 length only).")]
        public string MeterReadValue { get; set; }

        public List<ValidationResult> ValidationResults;

        public MeterReading(string HeaderData, string RowData)
        {
            _headerData = HeaderData;
            _rowData = RowData;

            LoadData();
            ValidateData();
        }

        public bool IsValid {
            get {
                return ValidationResults.Count == 0;
            }
        }

        private void LoadData()
        {
            string[] headersCols = _headerData.Split(COLUMN_DELIMITER);
            string[] rowCols = _rowData.Split(COLUMN_DELIMITER);

            for (int c = 0; c < headersCols.Length; c++)
            {
                string colName = headersCols[c];
                string colValue = rowCols[c];

                PropertyInfo prop = this.GetType().GetProperty(colName);
                prop.SetValue(this, colValue);
            }
        }

        private void ValidateData()
        {
            ValidationResults = new List<ValidationResult>();

            ValidationContext ctx = new ValidationContext(this);
            Validator.TryValidateObject(this, ctx, ValidationResults, true);
        }
    }
}