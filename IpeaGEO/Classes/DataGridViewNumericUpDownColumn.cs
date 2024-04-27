using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace IpeaGeo
{

    public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    {

        public DataGridViewNumericUpDownColumn() : base(new DataGridViewNumericUpDownCell()) { }

    }

    public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
    {
        public DataGridViewNumericUpDownCell() : base() { }

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewNumericUpDownControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                return typeof(decimal);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return 0;
            }
        }

        public override Type FormattedValueType
        {
            get
            {
                return typeof(decimal);
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, System.ComponentModel.TypeConverter valueTypeConverter, System.ComponentModel.TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            return value.ToString();
        }
        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            ((DataGridViewNumericUpDownControl)this.DataGridView.EditingControl).Value = decimal.Parse(this.Value.ToString());
        }

    }

    public class DataGridViewNumericUpDownControl : NumericUpDown, IDataGridViewEditingControl
    {

        private DataGridView _dataGridView;
        private int _rowIndex;
        private bool _valueChanged = false;

        #region IDataGridViewEditingControl Members

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;            
        }

        public DataGridView EditingControlDataGridView
        {
            get
            {
                return this._dataGridView;
            }
            set
            {
                this._dataGridView = value;
            }
        }

        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = decimal.Parse(value.ToString());
            }
        }

        public int EditingControlRowIndex
        {
            get
            {
                return this._rowIndex;
            }
            set
            {
                this._rowIndex = value;
            }
        }

        public bool EditingControlValueChanged
        {
            get
            {
                return this._valueChanged;
            }
            set
            {
                this._valueChanged = value;
            }
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch (keyData)
            {
                case Keys.Up:
                case Keys.Down:
                    return true;
                default:
                    return false;
            }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.EditingControlFormattedValue;
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            this._valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(e);
        }
        #endregion
    }

    //public class DataGridViewNumericUpDownColumn : DataGridViewColumn
    //{
    //    public DataGridViewNumericUpDownColumn() : base(new DataGridViewNumericUpDownCell())
    //    {
    //    }

    //    public override DataGridViewCell CellTemplate
    //    {
    //        get
    //        {
    //            return base.CellTemplate;
    //        }
    //        set
    //        {
    //            if (value != null &&
    //                !value.GetType().IsAssignableFrom(typeof(DataGridViewNumericUpDownCell)))
    //            {
    //                throw new InvalidCastException("Must be a DataGridViewNumericUpDownCell");
    //            }
    //            base.CellTemplate = value;
    //        }
    //    }
    //}

    //public class DataGridViewNumericUpDownCell : DataGridViewTextBoxCell
    //{

    //    public DataGridViewNumericUpDownCell()
    //        : base()
    //    {
    //    }

    //    public override void InitializeEditingControl(int rowIndex, object
    //        initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
    //    {
    //        // Set the value of the editing control to the current cell value.
    //        base.InitializeEditingControl(rowIndex, initialFormattedValue,
    //            dataGridViewCellStyle);
    //        DataGridViewNumericUpDownEditingControl ctl =
    //            DataGridView.EditingControl as DataGridViewNumericUpDownEditingControl;
    //        ctl.Value = (int)this.Value;
    //    }

    //    public override Type EditType
    //    {
    //        get
    //        {
    //            // Return the type of the editing contol that CalendarCell uses.
    //            return typeof(DataGridViewNumericUpDownEditingControl);
    //        }
    //    }

    //    public override Type ValueType
    //    {
    //        get
    //        {
    //            // Return the type of the value that CalendarCell contains.
    //            return typeof(decimal);
    //        }
    //    }

    //    public override object DefaultNewRowValue
    //    {
    //        get
    //        {
    //            // Use the current date and time as the default value.
    //            return 0d;
    //        }
    //    }
    //}

    //class DataGridViewNumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    //{
    //    DataGridView dataGridView;
    //    private bool valueChanged = false;
    //    int rowIndex;

    //    public DataGridViewNumericUpDownEditingControl()
    //    {
    //    }

    //    // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
    //    // property.
    //    public object EditingControlFormattedValue
    //    {
    //        get
    //        {
    //            return this.Value;
    //        }
    //        set
    //        {
    //            this.Value = decimal.Parse(value.ToString());
    //        }
    //    }

    //    // Implements the 
    //    // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
    //    public object GetEditingControlFormattedValue(
    //        DataGridViewDataErrorContexts context)
    //    {
    //        return EditingControlFormattedValue;
    //    }

    //    // Implements the 
    //    // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
    //    public void ApplyCellStyleToEditingControl(
    //        DataGridViewCellStyle dataGridViewCellStyle)
    //    {
    //        this.Font = dataGridViewCellStyle.Font;
    //    }

    //    // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
    //    // property.
    //    public int EditingControlRowIndex
    //    {
    //        get
    //        {
    //            return rowIndex;
    //        }
    //        set
    //        {
    //            rowIndex = value;
    //        }
    //    }

    //    // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
    //    // method.
    //    public bool EditingControlWantsInputKey(
    //        Keys key, bool dataGridViewWantsInputKey)
    //    {
    //        switch (key & Keys.KeyCode)
    //        {
    //            case Keys.Left:
    //            case Keys.Up:
    //            case Keys.Down:
    //            case Keys.Right:
    //            case Keys.Home:
    //            case Keys.End:
    //            case Keys.PageDown:
    //            case Keys.PageUp:
    //                return true;
    //            default:
    //                return false;
    //        }
    //    }

    //    // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
    //    // method.
    //    public void PrepareEditingControlForEdit(bool selectAll)
    //    {
    //        // No preparation needs to be done.
    //    }

    //    // Implements the IDataGridViewEditingControl
    //    // .RepositionEditingControlOnValueChange property.
    //    public bool RepositionEditingControlOnValueChange
    //    {
    //        get
    //        {
    //            return false;
    //        }
    //    }

    //    // Implements the IDataGridViewEditingControl
    //    // .EditingControlDataGridView property.
    //    public DataGridView EditingControlDataGridView
    //    {
    //        get
    //        {
    //            return dataGridView;
    //        }
    //        set
    //        {
    //            dataGridView = value;
    //        }
    //    }

    //    // Implements the IDataGridViewEditingControl
    //    // .EditingControlValueChanged property.
    //    public bool EditingControlValueChanged
    //    {
    //        get
    //        {
    //            return valueChanged;
    //        }
    //        set
    //        {
    //            valueChanged = value;
    //        }
    //    }

    //    // Implements the IDataGridViewEditingControl
    //    // .EditingPanelCursor property.
    //    public Cursor EditingPanelCursor
    //    {
    //        get
    //        {
    //            return base.Cursor;
    //        }
    //    }

    //    protected override void OnValueChanged(EventArgs eventargs)
    //    {
    //        // Notify the DataGridView that the contents of the cell
    //        // have changed.
    //        valueChanged = true;
    //        this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
    //        base.OnValueChanged(eventargs);
    //    }
    //}

}
