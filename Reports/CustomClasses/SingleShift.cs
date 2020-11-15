using System;
using System.Windows.Forms;

namespace Reports.CustomClasses
{
    class SingleShift
    {
        private readonly DataBase _dataBase;
        private readonly DataGridView _dataGridView;
        private readonly Label _label;

        public SingleShift(DataGridView dataGridView, Label label)
        {
            _dataBase = new DataBase();
            _dataGridView = dataGridView;
            _label = label;
        }        

        public void ReportByOtdel(int index, string tree, string dan, string gacha)
        {
            switch (index)
            {
                case 0:
                    _dataBase.getRecords("select *from getallevents_by_otdel('" + tree + "','" + dan + "','" +
                        gacha + "')", _dataGridView);
                    break;
                case 1:
                    _dataBase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel," +
                        "t2.lavozim, t1.kirish, t1.chiqish from reports t1 inner join employee t2 on " +
                        "t1.employeeid = t2.employeeid where t1.kirish::date >= '" + dan + "' and t1.kirish::date <= '" +
                        gacha + "' and t2.department  <@ '" + tree + "'", _dataGridView);
                    break;
                case 2:
                    _dataBase.getRecords("select *from getlate_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 3:
                    _dataBase.getRecords("select *from getearly_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 4:
                    _dataBase.getRecords("select *from getmissed_by_otdel('" + tree + "','" + 
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 5:
                    _dataBase.getRecords("select *from getworkedhours_total_by_otdel('" + tree +
                        "','" + dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 6:
                    _dataBase.getRecords("select *from getworked_hours_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 7:
                    _dataBase.getRecords("select *from get_extra_worked_hours_total_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 8:
                    _dataBase.getRecords("select *from get_extrawork_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 9:
                    _dataBase.getRecords("select *from getbeing_factory_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 10:
                    _dataBase.getRecords("select *from getsotrudniki_vnutri_day('" + DateTime.Now.ToString("yyyy-MM-dd") + "','" +
                    DateTime.Now.ToString("yyyy-MM-dd") + "')", _dataGridView);
                    break;
                case 11:
                    _dataBase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, " +
                    "t2.lavozim, t1.sabab, t1.dan, t1.gacha from otpusk t1 inner join employee t2 on t1.employeeid = " +
                    "t2.employeeid where (t2.department  <@ '" + tree + "' and dan >= '" +
                    dan + "' and dan <= '" + gacha + "') or (t2.department  <@ '" +
                    tree + "' and gacha >= '" + dan + "' and gacha <= '" +
                    gacha + "')", _dataGridView);
                    break;
                case 12:
                    _dataBase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, " +
                        "t1.door, t1.sana, t1.temperature from temperature t1 inner join employee t2 on t1.employeeid = " +
                        "t2.employeeid where t2.department <@ '" + tree + "' and t1.sana >= '" + dan + "' and " +
                        "t1.sana <= '" + gacha + "'", _dataGridView);
                    break;
            }
            GridHeaders(index);
            RowCnt(index);
        }

        public void ReportByPerson(int index, int id, string dan, string gacha)
        {
            switch (index)
            {
                case 0:
                    _dataBase.getRecords("select *from getallevents_by_person(" + id + ",'" +
                    dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 1:                  
                    _dataBase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel," +
                    "t2.lavozim, t1.kirish, t1.chiqish from reports t1 inner join employee t2 on " +
                    "t1.employeeid = t2.employeeid where t1.kirish::date >= '" + dan + "' and t1.kirish::date <= '" +
                    gacha + "' and t2.employeeid = " + id, _dataGridView);
                    break;
                case 2:
                    _dataBase.getRecords("select *from getlate_by_person(" + id + ",'" + dan + "','" + gacha +
                        "')", _dataGridView);
                    break;
                case 3:
                    _dataBase.getRecords("select *from getearly_by_person(" + id + ",'" + dan + "','" + gacha +
                        "')", _dataGridView);
                    break;
                case 4:
                    _dataBase.getRecords("select *from getmissed_by_person(" + id + ",'" + dan + "','" + gacha + 
                        "')", _dataGridView);
                    break;
                case 5:
                    _dataBase.getRecords("select *from getworked_hours_total_byperson(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 6:
                    _dataBase.getRecords("select *from getworked_hours_by_person(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 7:
                    _dataBase.getRecords("select *from get_extra_worked_hours_total_by_person(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 8:
                    _dataBase.getRecords("select *from get_extrawork_by_person(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 9:
                    _dataBase.getRecords("select *from getbeing_factory_by_person(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 11:
                    _dataBase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, " +
                    "t2.lavozim, t1.sabab, t1.dan, t1.gacha from otpusk t1 inner join employee t2 on t1.employeeid = " +
                    "t2.employeeid where (t1.employeeid = " + id + " and dan >= '" + dan + "' and dan <= '" +
                    gacha + "') or (t1.employeeid = " + id + " and gacha >= '" + dan + "' and gacha <= '" +
                    gacha + "')", _dataGridView);
                    break;
                case 12:
                    _dataBase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, t2.lavozim, " +
                        "t1.door, t1.sana, t1.temperature from temperature t1 inner join employee t2 on t1.employeeid = " +
                        "t2.employeeid where t1.employeeid = " + id + " and t1.sana >= '" + dan + "' and " +
                        "t1.sana <= '" + gacha + "'", _dataGridView);
                    break;
            }
            GridHeaders(index);
            //RowCnt(index);
        }

        private void GridHeaders(int index)
        {            
            switch (index)
            {
                case 0:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    _dataGridView.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_QUICK_OUTSIDE;
                    _dataGridView.Columns[10].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    _dataGridView.Columns[11].HeaderText = Properties.Resources.GRIDVIEW_LATE;
                    _dataGridView.Columns[12].HeaderText = Properties.Resources.GRIDVIEW_EXIT;
                    _dataGridView.Columns[13].HeaderText = Properties.Resources.GRIDVIEW_EARLY;
                    _dataGridView.Columns[14].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    _dataGridView.Columns[15].HeaderText = Properties.Resources.GRIDVIEW_MISSING;
                    break;

                case 1:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_EXIT;

                    break;
                case 2:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_LATE;
                    break;

                case 3:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_EXIT;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_EARLY;
                    break;

                case 4:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    break;

                case 5:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    _dataGridView.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    _dataGridView.Columns[10].HeaderText = Properties.Resources.GRIDVIEW_QUICK_OUTSIDE;
                    _dataGridView.Columns[11].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 6:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    _dataGridView.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_QUICK_OUTSIDE;
                    _dataGridView.Columns[10].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 7:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 8:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 9:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_HOLIDAY;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    _dataGridView.Columns[9].HeaderText = Properties.Resources.GRIDVIEW_QUICK_OUTSIDE;
                    _dataGridView.Columns[10].HeaderText = Properties.Resources.GRIDVIEW_ENTER;
                    _dataGridView.Columns[11].HeaderText = Properties.Resources.GRIDVIEW_EXIT;
                    _dataGridView.Columns[12].HeaderText = Properties.Resources.GRIDVIEW_HOURS;
                    break;

                case 10:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    break;

                case 11:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_VACATION;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_FROM;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_TO;
                    break;

                case 12:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = Properties.Resources.GRIDVIEW_SURNAME;
                    _dataGridView.Columns[2].HeaderText = Properties.Resources.GRIDVIEW_NAME;
                    _dataGridView.Columns[3].HeaderText = Properties.Resources.GRIDVIEW_FAMILY_NAME;
                    _dataGridView.Columns[4].HeaderText = Properties.Resources.GRIDVIEW_DEPARTMENT;
                    _dataGridView.Columns[5].HeaderText = Properties.Resources.GRIDVIEW_POSITION;
                    _dataGridView.Columns[6].HeaderText = Properties.Resources.GRIDVIEW_DOOR;
                    _dataGridView.Columns[7].HeaderText = Properties.Resources.GRIDVIEW_DATE;
                    _dataGridView.Columns[8].HeaderText = Properties.Resources.GRIDVIEW_TEMPERATURE;
                    break;

                default: break;
            }
        }

        private void RowCnt(int index)
        {
            switch (index)
            {
                case 0:
                    int otp = 0, opzd = 0, rann = 0, ots = 0;
                    foreach (DataGridViewRow row in _dataGridView.Rows)
                    {
                        if (!string.IsNullOrEmpty(row.Cells[8].Value.ToString())) // otpusk
                        {
                            otp++;
                        }

                        if (!string.IsNullOrEmpty(row.Cells[11].Value.ToString()))
                        {
                            opzd++;
                        }
                        if (!string.IsNullOrEmpty(row.Cells[13].Value.ToString()))
                        {
                            rann++;
                        }
                        if (!string.IsNullOrEmpty(row.Cells[15].Value.ToString()))
                        {
                            ots++;
                        }

                    }
                    _label.Text +=  Properties.Resources.VACATION + ": "   + otp +
                        "   |   " + Properties.Resources.LATE_COME + ": "  + opzd +
                        "   |   " + Properties.Resources.EARLY_GONE + ": " + rann +
                        "   |   " + Properties.Resources.MISSING + ": "    + ots;
                    break;
                case 1:
                    _label.Text += "    " + Properties.Resources.NUMBER_OF_EVENTS + ": " + _dataGridView.RowCount;
                    break;
                case 2:
                    _label.Text += "    " + Properties.Resources.LATE_COME + ": " + _dataGridView.RowCount;
                    break;
                case 3:
                    _label.Text += "    " + Properties.Resources.EARLY_GONE + ": " + _dataGridView.RowCount;
                    break;
                case 4:
                    _label.Text += "    " + Properties.Resources.MISSING + ": " + _dataGridView.RowCount;
                    break;
                case 10:
                    _label.Text += "    " + Properties.Resources.EMPLOYEE_INSIDE + ": " + _dataGridView.RowCount;
                    break;
                case 11:
                    _label.Text += "    " + Properties.Resources.HOLIDAY + "&" + Properties.Resources.VACATION + ": " + _dataGridView.RowCount;
                    break;
                default: _label.Text = ""; break;
            }
        }
    }
}
