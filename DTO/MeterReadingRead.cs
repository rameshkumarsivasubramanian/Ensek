using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Ensek.DTO
{
    public class MeterReadingRead
    {
        const char COLUMN_DELIMITER = ',';
        private readonly int _rowNum;
        private readonly string _headerData;
        private readonly string _rowData;
        
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Invalid {0}(numbers only).")]
        public string AccountId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public string MeterReadingDateTime { get; set; }
        [Required]
        //07. Reading values should be in the format NNNNN
        [RegularExpression("^[0-9]{5}$", ErrorMessage = "{0} should be in the format NNNNN.")]
        public string MeterReadValue { get; set; }

        public IEnumerable<string> ValidationResults;

        public MeterReadingRead(int RowNum, string HeaderData, string RowData)
        {
            _rowNum = RowNum;
            _headerData = HeaderData;
            _rowData = RowData;

            //Dynamically read column data. The columns can be in any order.
            LoadData();
            //03. Each entry in the CSV should be validated
            ValidateData();
        }

        public bool IsValid {
            get {
                return ValidationResults.Count() == 0;
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
            List<ValidationResult> results = new List<ValidationResult>();

            ValidationContext ctx = new ValidationContext(this);
            Validator.TryValidateObject(this, ctx, results, true);

            ValidationResults = results.Select(v => "Row#" + _rowNum + " " + v.ErrorMessage);
        }
    }
}