using System.Windows.Forms;

namespace Reports.CustomClasses
{
    class SingleShift
    {
        private readonly MyDatabase _myDatabase;
        private readonly DataGridView _dataGridView;
        private readonly Label _label;

        public SingleShift(DataGridView dataGridView, Label label)
        {
            _myDatabase = new MyDatabase();
            _dataGridView = dataGridView;
            _label = label;
        }        

        public void ReportByOtdel(int index, string tree, string dan, string gacha)
        {
            switch (index)
            {
                case 0:
                    _myDatabase.getRecords("select *from getallevents_by_otdel('" + tree + "','" + dan + "','" +
                        gacha + "')", _dataGridView);
                    break;
                case 1:
                    _myDatabase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel," +
                        "t2.lavozim, t1.kirish, t1.chiqish from reports t1 inner join employee t2 on " +
                        "t1.employeeid = t2.employeeid where t1.kirish::date >= '" + dan + "' and t1.kirish::date <= '" +
                        gacha + "' and t2.department  <@ '" + tree + "'", _dataGridView);
                    break;
                case 2:
                    _myDatabase.getRecords("select *from getlate_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 3:
                    _myDatabase.getRecords("select *from getearly_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 4:
                    _myDatabase.getRecords("select *from getmissed_by_otdel('" + tree + "','" + 
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 5:
                    _myDatabase.getRecords("select *from getworkedhours_total_by_otdel('" + tree +
                        "','" + dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 6:
                    _myDatabase.getRecords("select *from getworked_hours_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 7:
                    _myDatabase.getRecords("select *from getbeing_factory_by_otdel('" + tree + "','" +
                        dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 8:
                    _myDatabase.getRecords("select *from getsotrudniki_vnutri_day('" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 9:
                    _myDatabase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, " +
                    "t2.lavozim, t1.sabab, t1.dan, t1.gacha from otpusk t1 inner join employee t2 on t1.employeeid = " +
                    "t2.employeeid where (t2.department  <@ '" + tree + "' and dan >= '" +
                    dan + "' and dan <= '" + gacha + "') or (t2.department  <@ '" +
                    tree + "' and gacha >= '" + dan + "' and gacha <= '" +
                    gacha + "')", _dataGridView);
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
                    _myDatabase.getRecords("select *from getallevents_by_person(" + id + ",'" +
                    dan + "','" + gacha + "')", _dataGridView);
                    break;
                case 1:                  
                    _myDatabase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel," +
                    "t2.lavozim, t1.kirish, t1.chiqish from reports t1 inner join employee t2 on " +
                    "t1.employeeid = t2.employeeid where t1.kirish::date >= '" + dan + "' and t1.kirish::date <= '" +
                    gacha + "' and t2.employeeid = " + id, _dataGridView);
                    break;
                case 2:
                    _myDatabase.getRecords("select *from getlate_by_person(" + id + ",'" + dan + "','" + gacha +
                        "')", _dataGridView);
                    break;
                case 3:
                    _myDatabase.getRecords("select *from getearly_by_person(" + id + ",'" + dan + "','" + gacha +
                        "')", _dataGridView);
                    break;
                case 4:
                    _myDatabase.getRecords("select *from getmissed_by_person(" + id + ",'" + dan + "','" + gacha + 
                        "')", _dataGridView);
                    break;
                case 5:
                    _myDatabase.getRecords("select *from getworked_hours_total_byperson(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 6:
                    _myDatabase.getRecords("select *from getworked_hours_by_person(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 7:
                    _myDatabase.getRecords("select *from getbeing_factory_by_person(" + id + ",'" + dan + "','" +
                    gacha + "')", _dataGridView);
                    break;
                case 9:
                    _myDatabase.getRecords("select t2.employeeid, t2.familiya, t2.ism, t2.otchestvo, t2.otdel, " +
                    "t2.lavozim, t1.sabab, t1.dan, t1.gacha from otpusk t1 inner join employee t2 on t1.employeeid = " +
                    "t2.employeeid where (t1.employeeid = " + id + " and dan >= '" + dan + "' and dan <= '" +
                    gacha + "') or (t1.employeeid = " + id + " and gacha >= '" + dan + "' and gacha <= '" +
                    gacha + "')", _dataGridView);
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
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "День";
                    _dataGridView.Columns[7].HeaderText = "Праздник";
                    _dataGridView.Columns[8].HeaderText = "Отпуск";
                    _dataGridView.Columns[9].HeaderText = "Увольнительные";
                    _dataGridView.Columns[10].HeaderText = "Вход";
                    _dataGridView.Columns[11].HeaderText = "Опоздал/а";
                    _dataGridView.Columns[12].HeaderText = "Выход";
                    _dataGridView.Columns[13].HeaderText = "Ранний";
                    _dataGridView.Columns[14].HeaderText = "Часы в день";
                    _dataGridView.Columns[15].HeaderText = "Отсутствует";
                    break;

                case 1:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "Вход";
                    _dataGridView.Columns[7].HeaderText = "Выход";
                    break;
                case 2:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "День";
                    _dataGridView.Columns[7].HeaderText = "Вход";
                    _dataGridView.Columns[8].HeaderText = "Опоздал/а";
                    break;
                case 3:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "День";
                    _dataGridView.Columns[7].HeaderText = "Выход";
                    _dataGridView.Columns[8].HeaderText = "Ранний";
                    break;
                case 4:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "День";
                    _dataGridView.Columns[7].HeaderText = "Праздник";
                    _dataGridView.Columns[8].HeaderText = "Отпуск";
                    break;

                case 5:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "От";
                    _dataGridView.Columns[7].HeaderText = "До";
                    _dataGridView.Columns[8].HeaderText = "Праздник";
                    _dataGridView.Columns[9].HeaderText = "Отпуск";
                    _dataGridView.Columns[10].HeaderText = "Увольнительные";
                    _dataGridView.Columns[11].HeaderText = "Общ часы";
                    break;

                case 6:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "День";
                    _dataGridView.Columns[7].HeaderText = "Праздник";
                    _dataGridView.Columns[8].HeaderText = "Отпуск";
                    _dataGridView.Columns[9].HeaderText = "Увольнительные";
                    _dataGridView.Columns[10].HeaderText = "Часы в день";
                    break;

                case 7:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "День";
                    _dataGridView.Columns[7].HeaderText = "Праздник";
                    _dataGridView.Columns[8].HeaderText = "Отпуск";
                    _dataGridView.Columns[9].HeaderText = "Увольнительные";
                    _dataGridView.Columns[10].HeaderText = "Вход";
                    _dataGridView.Columns[11].HeaderText = "Выход";
                    _dataGridView.Columns[12].HeaderText = "Время присутствия";
                    break;

                case 8:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "Число";
                    break;

                case 9:
                    _dataGridView.Columns[0].HeaderText = "ID";
                    _dataGridView.Columns[1].HeaderText = "Фамилия";
                    _dataGridView.Columns[2].HeaderText = "Имя";
                    _dataGridView.Columns[3].HeaderText = "Отчество";
                    _dataGridView.Columns[4].HeaderText = "Отдел";
                    _dataGridView.Columns[5].HeaderText = "Должность";
                    _dataGridView.Columns[6].HeaderText = "Отпуск";
                    _dataGridView.Columns[7].HeaderText = "От";
                    _dataGridView.Columns[8].HeaderText = "До";
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
                        if (!string.IsNullOrEmpty(row.Cells[8].Value.ToString()))
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
                    _label.Text += "  сотрудники =>  в больн/отпуске: " + otp + "     Опоздавшие: " + opzd + "     Ранние уходы: " +
                        rann + "     Отсутсвующие: " + ots;
                    break;
                case 1:
                    _label.Text += "    количество события: " + _dataGridView.RowCount;
                    break;
                case 2:
                    _label.Text += "    Опоздавшие: " + _dataGridView.RowCount;
                    break;
                case 3:
                    _label.Text += "   Ранние уходы: " + _dataGridView.RowCount;
                    break;
                case 4:
                    _label.Text += "   Отсутсвующие: " + _dataGridView.RowCount;
                    break;
                case 8:
                    _label.Text += "   Сотрудники - внутри: " + _dataGridView.RowCount;
                    break;
                case 9:
                    _label.Text += "   Отпуск - больничный: " + _dataGridView.RowCount;
                    break;
                default: _label.Text = ""; break;
            }
        }
    }
}
