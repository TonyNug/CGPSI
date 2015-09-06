using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CGPSI.AbsenceManagement.Models
{
    public class KendoHelper
    {
        /// <summary>
        /// fungsi ini digunakan untuk mengenerate filter yang digunakan sebagai parameter where dan untuk paging hasil query pada database
        /// inputan di dapatkan dari hasil posting ajax kendo ui dan menghasilkan string yang terbagi menjadi dua bagian yaitu 
        /// string untuk paging dan string untuk kondisi where,
        /// dua string tersebut digabungkan menjadi satu string dan dipisahkan dengan char "|" 
        /// susunan string tersebut adalah string_paging|string_where
        /// </summary>
        public static string GenerateFilters(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter)
        {
            string hasil = string.Empty;
            string where = string.Empty;
            string paging = string.Empty;
            string sorting = string.Empty;

            int pageNum = (page.ToString().Length > 0) ? int.Parse(page) : 0;
            int limit = (take.ToString().Length > 0) ? int.Parse(take.ToString()) : 0;

            if (sort != null)
            {
                sorting = (sort.Count() > 0) ? QueryBuilder.buildSortingFilter(sort) : "";
            }
            where += QueryBuilder.GenerateWhereFilter(filter, where);

            paging = (limit.ToString().Length > 0 || pageNum.ToString().Length > 0) ? " " + QueryBuilder.buildPagingFilter(int.Parse(pageNum.ToString()), int.Parse(limit.ToString())) + " " : "";
            hasil += sorting + " ;" + paging + " | " + where + " ";
            return hasil;
        }


        /// <summary>
        /// Fungsi ini digunakan untuk menggenerate string filter dengan mengabaikan salah satu field berdasarkan inputan agar tidak dimasukkan kedalam query
        /// </summary>
        public static string GenerateFiltersIgnore(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter, string field)
        {
            string hasil = string.Empty;
            string where = string.Empty;
            string paging = string.Empty;
            string sorting = string.Empty;

            int pageNum = (page.ToString().Length > 0) ? int.Parse(page) : 0;
            int limit = (take.ToString().Length > 0) ? int.Parse(take.ToString()) : 0;

            if (sort != null)
            {
                sorting = (sort.Count() > 0) ? QueryBuilder.buildSortingFilter(sort) : "";
            }
            where += QueryBuilder.GenerateWhereFilterIgnore(filter, where, field);

            paging = (limit.ToString().Length > 0 || pageNum.ToString().Length > 0) ? " " + QueryBuilder.buildPagingFilter(int.Parse(pageNum.ToString()), int.Parse(limit.ToString())) + " " : "";
            hasil += sorting + " ;" + paging + " | " + where + " ";
            return hasil;
        }

        public static string GetValueOfFilter(string field, GridFilter filter)
        {
            return QueryBuilder.GetValueFromFieldFilter(filter, field, "");
        }

        public static string GenerateFiltersCustomTableIgnored(string take, string page, string skip, string pageSize, List<GridSort> sort, GridFilter filter, string ignored, string customField)
        {
            string hasil = string.Empty;
            //Dictionary<string, object> Filter = JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
            // List<string> Filter = JsonConvert.DeserializeObject<List<string>>(input);
            string where = string.Empty;
            string paging = string.Empty;
            string sorting = string.Empty;

            int pageNum = (page.ToString().Length > 0) ? int.Parse(page) : 0;
            int limit = (take.ToString().Length > 0) ? int.Parse(take.ToString()) : 0;

            if (sort != null)
            {
                sorting = (sort.Count() > 0) ? QueryBuilder.buildSortingFilter(sort) : "";
            }
            where += QueryBuilder.GenerateWhereFilterCustomFieldIgnored(filter, where, ignored, customField);

            paging = (limit.ToString().Length > 0 || pageNum.ToString().Length > 0) ? " " + QueryBuilder.buildPagingFilter(int.Parse(pageNum.ToString()), int.Parse(limit.ToString())) + " " : "";
            hasil += sorting + " ;" + paging + " | " + where + " ";
            return hasil;
        }
    }

    public class GridResult
    {
        public string Result { get; set; }
        public string CResult { get; set; }
    }

    public class GridFilter
    {
        public string Operator { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
        public string Logic { get; set; }
        public List<GridFilter> Filters { get; set; }
    }

    public class GridFilters
    {
        public List<GridFilter> Filters { get; set; }
        public string Logic { get; set; }
    }

    public class GridSort
    {
        public string Field { get; set; }
        public string Dir { get; set; }
    }





    public static class QueryBuilder
    {
        /// <summary>
        /// Fungsi ini digunakan untuk melakukan pengecekan apakan inputan termasuk dalam type Date atau bukan
        /// </summary>
        public static bool isDate(string input)
        {
            DateTime temp = DateTime.Now;
            return DateTime.TryParse(input, out temp);
        }

        public static bool isBoolean(string input)
        {
            return (input.ToString().ToLower() == "true" || input.ToString().ToLower() == "false") ? true : false;
        }

        public static string buildFilterItem(string field, string op, string value)
        {
            op = (isBoolean(value) == true) ? "bool" : op;


            string temp = string.Empty;
            try
            {
                switch (op)
                {
                    #region stringType

                    case "s_eq": { temp = field + " = '" + value + "'"; break; }
                    case "s_neq": { temp = field + " != '" + value + "'"; break; }
                    case "doesnotcontain": { temp = field + " not like '%" + value + "%'"; break; }
                    case "contains": { temp = field + " like '%" + value + "%'"; break; }
                    case "startswith": { temp = field + " like '" + value + "%'"; break; }
                    case "endswith": { temp = field + " like '%" + value + "'"; break; }

                    #endregion
                    #region DateType
                    case "d_eq": { temp = field + " = '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "d_gt": { temp = field + " > '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "d_gte": { temp = field + " >= '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "d_lt": { temp = field + " < '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "d_lte": { temp = field + " <= '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "d_neq": { temp = field + " != '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }

                    #endregion

                    #region numberType
                    case "n_eq": { temp = field + " = '" + value + "'"; break; }
                    case "n_gt": { temp = field + " > '" + value + "'"; break; }
                    case "n_gte": { temp = field + " >= '" + value + "'"; break; }
                    case "n_lt": { temp = field + " < '" + value + "'"; break; }
                    case "n_lte": { temp = field + " <= '" + value + "'"; break; }
                    case "n_neq": { temp = field + " != '" + value + "'"; break; }
                    #endregion

                    case "bool":
                        {
                            int num = 0;
                            num = (value.ToLower() == "true") ? 1 : 0;
                            temp = field + " = '" + num + "'";
                            break;
                        }

                    default: { temp = field + " = '" + value + "'"; break; }
                }
            }
            catch { }
            return temp;
        }

        /// <summary>
        /// Fungsi ini digunakan untuk melakukan pengecekan apakan inputan termasuk dalam type Number atau bukan
        /// </summary>
        public static bool isNumber(string input)
        {
            bool temp = false;
            int tempInt = 0;
            decimal tempDec;
            if ((int.TryParse(input, out tempInt) == true) || decimal.TryParse(input, out tempDec) == true) temp = true;
            return temp;
        }

        public static string getStringOperator(string input)
        {
            string temp = string.Empty;
            try
            {
                switch (input)
                {
                    case "eq": { temp = "LIKE"; break; }
                    case "neq": { temp = "NOT LIKE"; break; }
                    case "doesnotcontain": { temp = "NOT LIKE"; break; }
                    case "contains": { temp = "LIKE"; break; }
                    case "startswith": { temp = "LIKE"; break; }
                    case "endswith": { temp = "LIKE"; break; }
                    default: { temp = "LIKE"; break; }
                }
            }
            catch { }
            return temp;
        }

        /// <summary>
        /// Fungsi ini digunakan untuk mengenerate sebuah filter (type field string) berdasarkan inputan field(nama kolom), op(operator), value(nilai yang dicari)"
        /// </summary>
        public static string buildFilterItem_string(string field, string op, string value)
        {
            string temp = string.Empty;
            try
            {
                switch (op)
                {
                    case "eq": { temp = field + " = '" + value + "'"; break; }
                    case "neq": { temp = field + " not like '" + value + "'"; break; }
                    case "doesnotcontain": { temp = field + " not like '%" + value + "%'"; break; }
                    case "contains": { temp = field + " like '%" + value + "%'"; break; }
                    case "startswith": { temp = field + " like '" + value + "%'"; break; }
                    case "endswith": { temp = field + " like '%" + value + "'"; break; }
                    default: { temp = field + " like '" + value + "'"; break; }
                }
            }
            catch { }
            return temp;
        }

        /// <summary>
        /// Fungsi ini digunakan untuk mengenerate sebuah filter (type field Date) berdasarkan inputan field(nama kolom), op(operator), value(nilai yang dicari)"
        /// </summary>
        public static string buildFilterItem_date(string field, string op, string value)
        {
            string temp = string.Empty;
            try
            {
                switch (op)
                {
                    case "eq": { temp = field + " = '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "gt": { temp = field + " > '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "gte": { temp = field + " >= '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "lt": { temp = field + " < '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "lte": { temp = field + " <= '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                    case "neq": { temp = field + " != '" + DateTime.Parse(value).ToShortDateString() + "'"; break; }
                }
            }
            catch { }
            return temp;
        }

        /// <summary>
        /// Fungsi ini digunakan untuk mengenerate sebuah filter (type field Number) berdasarkan inputan field(nama kolom), op(operator), value(nilai yang dicari)"
        /// </summary>
        public static string buildFilterItem_number(string field, string op, string value)
        {
            string temp = string.Empty;
            try
            {
                switch (op)
                {
                    case "eq": { temp = field + " = '" + value + "'"; break; }
                    case "gt": { temp = field + " > '" + value + "'"; break; }
                    case "gte": { temp = field + " >= '" + value + "'"; break; }
                    case "lt": { temp = field + " < '" + value + "'"; break; }
                    case "lte": { temp = field + " <= '" + value + "'"; break; }
                    case "neq": { temp = field + " != '" + value + "'"; break; }
                }
            }
            catch { }
            return temp;
        }

        public static string buildFilterItem_boolean(string field, string op, string value)
        {
            string temp = string.Empty;
            string value_temp = (value.ToString().ToLower() == "true") ? "1" : "0";
            try
            {
                switch (op)
                {
                    case "eq": { temp = field + " = '" + value_temp + "'"; break; }
                    case "gt": { temp = field + " > '" + value_temp + "'"; break; }
                    case "gte": { temp = field + " >= '" + value_temp + "'"; break; }
                    case "lt": { temp = field + " < '" + value_temp + "'"; break; }
                    case "lte": { temp = field + " <= '" + value_temp + "'"; break; }
                    case "neq": { temp = field + " != '" + value_temp + "'"; break; }
                }
            }
            catch { }
            return temp;
        }

        /// <summary>
        /// Fungsi ini digunakan untuk mengenerate filter query guna proses paging pada datagrid.
        /// </summary>
        public static string buildPagingFilter(int PageNum, int LimitItem)
        {
            string temp = string.Empty;
            try
            {
                int temp1 = (PageNum - 1) * LimitItem;
                int temp2 = PageNum * LimitItem;
                temp = " and RowNum >" + temp1.ToString() + " and RowNum <= " + temp2.ToString() + " ";
            }
            catch { }
            return temp;
        }

        /// <summary>
        /// Fungsi ini digunakan untuk mengenerate filter query guna proses sorting pada datagrid.
        /// </summary>
        public static string buildSortingFilter(List<GridSort> input)
        {
            string output = string.Empty;
            //string dir = "ASC";
            string field = string.Empty;
            List<string> temp = new List<string>();
            try
            {
                if (input.Count() > 0)
                {
                    output += "ORDER BY  ";

                    foreach (GridSort item in input)
                    {
                        temp.Add(item.Field + " " + item.Dir + " ");
                    }
                    field = string.Join(",", temp.ToArray());
                }
            }
            catch { }
            return output + field;
        }

        /// <summary>
        /// Fungsi ini digunakan untuk mengenerate filter query berdasarkan nama columns, operator dan value/nilai nya
        /// </summary>
        public static string GenerateWhereFilter_b(GridFilters filter)
        {
            string output = string.Empty;//berisi query bentukan
            string logic = string.Empty; //berisi "and" atau "or"
            List<string> temp = new List<string>(); //digunakan untuk menampung hasil buildFilterItem dalam bentuk string
            string field = string.Empty;
            try
            {
                logic = filter.Logic;
                if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
                {
                    //string whereClause = null;
                    var parameters = new List<object>();
                    var filters = filter.Filters;
                    if (filters.Count > 0)
                    {
                        for (int i = 0; i < filters.Count; i++)
                        {
                            if (isDate(filters[i].Value.ToString()) == true)
                            {
                                temp.Add(buildFilterItem_date(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                            }
                            else if (isNumber(filters[i].Value.ToString()) == true)
                            {
                                temp.Add(buildFilterItem_number(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                            }
                            else if (isBoolean(filters[i].Value.ToString()) == true)
                            {
                                temp.Add(buildFilterItem_boolean(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                            }
                            else
                            {
                                temp.Add(buildFilterItem_string(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                            }
                        }

                        field = string.Join(" " + logic + " ", temp.ToArray());
                    }
                }
                else
                {
                    field = " ";
                }
            }
            catch { }
            return (field.Trim().Length > 0) ? " and " + field : " ";
        }

        public static string GenerateWhereFilter(GridFilter filter, string output_where)
        {
            string output = string.Empty;//berisi query bentukan
            string logic = string.Empty; //berisi "and" atau "or"
            List<string> temp = new List<string>();
            string field = string.Empty;
            try
            {
                logic = filter.Logic;
                if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
                {
                    //string whereClause = null;
                    var parameters = new List<object>();
                    var filters = filter.Filters;


                    for (int i = 0; i < filters.Count; i++)
                    {
                        if (filters[i].Filters == null)
                        {
                            if (filters[i].Value != null)
                            {
                                temp.Add(buildFilterItem(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                /* if (isDate(filters[i].Value.ToString()) == true)
                                 {
                                     temp.Add(buildFilterItem_date(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                 }
                                 else if (isNumber(filters[i].Value.ToString()) == true && filters[i].Field.ToLower() != "emplid" && filters[i].Field.ToLower() != "v.to_emplid" 
                                     && filters[i].Field.ToLower() != "to_empl_id" && filters[i].Field.ToLower() != "from_empl_id")
                                 {
                                     temp.Add(buildFilterItem_number(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                 }
                                 else if (isBoolean(filters[i].Value.ToString()))
                                 {
                                     temp.Add(buildFilterItem_boolean(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                 }
                                 else
                                 {
                                     temp.Add(buildFilterItem_string(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                 }*/
                            }
                        }
                        else
                        {
                            output_where += GenerateWhereFilter(filters[i], output_where);
                        }
                    }
                    field = string.Join(" " + logic + " ", temp.ToArray());

                }
            }
            catch { }
            output_where += (field.Trim().Length > 0) ? logic + "  " + field + " " : " ";
            return output_where;
        }

        public static string GenerateWhereFilterCustomFieldIgnored(GridFilter filter, string output_where, string ignored, string customField)
        {
            string output = string.Empty;//berisi query bentukan
            string logic = string.Empty; //berisi "and" atau "or"
            List<string> temp = new List<string>();
            string field = string.Empty;
            try
            {
                logic = filter.Logic;
                if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
                {
                    //string whereClause = null;
                    var parameters = new List<object>();
                    var filters = filter.Filters;
                    for (int i = 0; i < filters.Count; i++)
                    {
                        if (filters[i].Filters == null)
                        {
                            if (filters[i].Value != null)
                            {
                                if (filters[i].Field.ToLower() != ignored.ToLower())
                                {
                                    temp.Add(buildFilterItem(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //if (isDate(filters[i].Value.ToString()) == true)
                                    //{
                                    //    temp.Add(buildFilterItem_date(customField+filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                    //else if (isNumber(filters[i].Value.ToString()) == true && filters[i].Field.ToLower() != "emplid" && filters[i].Field.ToLower() != "v.to_emplid"
                                    //    && filters[i].Field.ToLower() != "to_empl_id" && filters[i].Field.ToLower() != "from_empl_id")
                                    //{
                                    //    temp.Add(buildFilterItem_number(customField+filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                    //else if (isBoolean(filters[i].Value.ToString()))
                                    //{
                                    //    temp.Add(buildFilterItem_boolean(customField+filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                    //else
                                    //{
                                    //    temp.Add(buildFilterItem_string(customField+filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                }
                            }
                        }
                        else
                        {
                            output_where += GenerateWhereFilterCustomFieldIgnored(filters[i], output_where, ignored, customField);
                        }

                    }
                    field = string.Join(" " + logic + " ", temp.ToArray());

                }
            }
            catch { }
            output_where += (field.Trim().Length > 0) ? logic + "  " + field + " " : " ";
            return output_where;
        }

        public static string GenerateWhereFilterIgnore(GridFilter filter, string output_where, string field_name)
        {
            string output = string.Empty;//berisi query bentukan
            string logic = string.Empty; //berisi "and" atau "or"
            List<string> temp = new List<string>();
            string field = string.Empty;
            try
            {
                logic = filter.Logic;
                if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
                {
                    //string whereClause = null;
                    var parameters = new List<object>();
                    var filters = filter.Filters;


                    for (int i = 0; i < filters.Count; i++)
                    {
                        if (filters[i].Filters == null)
                        {
                            if (filters[i].Value != null)
                            {
                                if (filters[i].Field.ToLower() != field_name.ToLower())
                                {
                                    temp.Add(buildFilterItem(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //if (isDate(filters[i].Value.ToString()) == true)
                                    //{
                                    //    temp.Add(buildFilterItem_date(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                    //else if (isNumber(filters[i].Value.ToString()) == true && filters[i].Field.ToLower() != "emplid" && filters[i].Field.ToLower() != "v.to_emplid"
                                    //    && filters[i].Field.ToLower() != "to_empl_id" && filters[i].Field.ToLower() != "from_empl_id")
                                    //{
                                    //    temp.Add(buildFilterItem_number(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                    //else if (isBoolean(filters[i].Value.ToString()))
                                    //{
                                    //    temp.Add(buildFilterItem_boolean(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                    //else
                                    //{
                                    //    temp.Add(buildFilterItem_string(filters[i].Field.ToString(), filters[i].Operator.ToString(), filters[i].Value.ToString()));
                                    //}
                                }
                            }
                        }
                        else
                        {
                            output_where += GenerateWhereFilterIgnore(filters[i], output_where, field_name);
                        }

                    }
                    field = string.Join(" " + logic + " ", temp.ToArray());

                }
            }
            catch { }
            output_where += (field.Trim().Length > 0) ? logic + "  " + field + " " : " ";
            return output_where;
        }

        public static string GetValueFromFieldFilter(GridFilter filter, string field, string output_where)
        {
            string output = string.Empty;//berisi query bentukan
            List<string> temp = new List<string>();
            try
            {
                if (filter != null && (filter.Filters != null && filter.Filters.Count > 0))
                {
                    //string whereClause = null;
                    var parameters = new List<object>();
                    var filters = filter.Filters;
                    for (int i = 0; i < filters.Count; i++)
                    {
                        if (filters[i].Filters == null)
                        {
                            if (filters[i].Value != null)
                            {
                                if (filters[i].Field.ToString() == field)
                                {
                                    output_where = filters[i].Value.ToString();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            output_where = GetValueFromFieldFilter(filters[i], field, output_where);
                        }
                    }
                }
            }
            catch { }
            return output_where;
        }


    }
}