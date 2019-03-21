/*
 * Developer:   Randy Lefebvre - 6842256
 * Program:     Advanced SQL - PROG3070
 * Description: This program is a windows forms program. It displays a graph on the FoodMart_2008 database.
 *              It gives the user the ability to view all sales of 2008, or refine their search on city,
 *              product class, or product. The UI only contains 3 combo boxes and a graph. 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;

namespace Sql_Assignment_04
{
    public partial class form_title : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["FoodMart_2008Sales"].ConnectionString;
        string productClass;
        int productClassID;
        string product;
        string city;
        public form_title()
        {
            InitializeComponent();
            PopulateComboBoxes();
            UpdateGraph();
        }

        /// <summary>
        /// This method will be called when the form starts. It will call two
        /// methods that populate each combo box
        /// </summary>
        private void PopulateComboBoxes()
        {
            PopulateComboClass();
            PopulateComboProduct("");
            PopulateComboCity();


            // Register the event handler for the Product class combo box. 
            // When a selection has been changed, we want to update the product combo box.
            cb_Class.SelectedIndexChanged +=
                new System.EventHandler(Product_Class_ComboBox_SelectedIndexChanged);
            cb_Product.SelectedIndexChanged +=
                new System.EventHandler(StoreProduct);
            cb_City.SelectedIndexChanged +=
                new System.EventHandler(StoreCity);

            DefaultSelection();
        }

        private void DefaultSelection()
        {
            cb_Class.SelectedIndex = 0;
            cb_City.SelectedIndex = 0;
            cb_Product.SelectedIndex = 0;
        }

        //  //////////////////////////////////////////////////////////////////////////
        //                           EVENT HANDLERS
        //  //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Used to handle the selection change of combo box city.
        /// Stores a global variable city, and updates the graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoreCity(object sender, EventArgs e)
        {
            city = cb_City.SelectedItem.ToString();
            UpdateGraph();
        }

        /// <summary>
        /// Used to handle the selection change of combo box product.
        /// Stores a global variable product, and updates the graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StoreProduct(object sender, EventArgs e)
        {
            product = cb_Product.SelectedItem.ToString();
            UpdateGraph();
        }

        /// <summary>
        /// Used to handle the selection change of combo box product class.
        /// Stores a global variable product class, and updates the graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Product_Class_ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            productClass = cb_Class.SelectedItem.ToString();
            string productID = string.Empty;
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                db.Open();
                cmd.Connection = db;
                cmd.CommandText = "SELECT product_class_id FROM product_class WHERE product_subcategory = '" + productClass +"'";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    productID = reader["product_class_id"].ToString();
                    productClassID = Int32.Parse(productID.ToString());
                    PopulateComboProduct(productID);
                }
                reader.Close();
                cmd.Dispose();
            }
            UpdateGraph();
        }

        //  //////////////////////////////////////////////////////////////////////////
        //                           POPULATE COMBO BOXES
        //  //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Used to populate the combo box for product class. 
        /// </summary>
        private void PopulateComboClass()
        {
            cb_Class.Items.Clear();
            productClass = "All";
            cb_Class.Items.Add("All");
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                db.Open();
                cmd.Connection = db;
                cmd.CommandText = "SELECT product_subcategory FROM product_class";
                SqlDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    cb_Class.Items.Add(reader["product_subcategory"].ToString());
                }
                reader.Close();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// Used to populate the combo box for product. Uses the global variable of
        /// the selected product class ID
        /// </summary>
        /// <param name="SelectedClassID"></param>
        private void PopulateComboProduct(string SelectedClassID)
        {
            cb_Product.Items.Clear();
            product = "All";
            cb_Product.Items.Add("All");
            // This is used to check if the Product Class has a selected product class.. 
            // If it doesnt, we dont want to do anything
            if (cb_Class.SelectedIndex > -1)
            {
                cb_Product.Enabled = true;

                using (SqlConnection db = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    db.Open();
                    cmd.Connection = db;
                    if (SelectedClassID.ToString() != "")
                    {
                        cmd.CommandText = "SELECT product_name FROM product WHERE product_class_id = " + SelectedClassID;
                    }
                    else
                    {
                        cmd.CommandText = "SELECT * FROM product ";
                    }
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cb_Product.Items.Add(reader["product_name"].ToString());
                    }
                    reader.Close();
                    cmd.Dispose();
                }
            }
            else
            {
                cb_Product.Enabled = false;
            }
        }

        /// <summary>
        /// Used to populate the combo box for city
        /// </summary>
        private void PopulateComboCity()
        {
            cb_City.Items.Clear();
            city = "All";
            cb_City.Items.Add("All");
            using (SqlConnection db = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                db.Open();
                cmd.Connection = db;
                cmd.CommandText = "SELECT * FROM store";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cb_City.Items.Add(reader["store_city"].ToString());
                }
                reader.Close();
                cmd.Dispose();
            }
        }


        //  //////////////////////////////////////////////////////////////////////////
        //                           UPDATE GRAPH
        //  //////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// This method will be called to update the graph depending on the selections choosen.
        /// This method is the meat and potatoes of the program. It creates the command string
        /// depending on what selection is made, then populates the reader. The reader then gathers
        /// the total cost and multiplies it with the currently of which is was sold to. It finally
        /// makes a call of the graph point maker to actually put the points on the graph.
        /// </summary>
        private void UpdateGraph()
        {
            chart_main.Titles.Clear();
            chart_main.Series[0].Points.Clear();
            LoadXAxisTitles();
            LoadYAxisTitles();
            
            using (SqlConnection db = new SqlConnection(connectionString))
            {


                //SqlCommand cmd = new SqlCommand();
                db.Open();
                cmd.Connection = db;


                // This if-else block is to create the command string depending on the selection that was chosen.
                // It will create different string depending on the data that it knows of. This will ensure we do not
                // get a crash for trying to make a command that we dont have access too, as well as lowers that need
                // to add extra lines to the command line.
                if ((product == "All") && (productClass == "All") && (city == "All"))
                {
                    cmd.CommandText = "SELECT * FROM sales_fact_1998";
                    //// Main title
                    chart_main.Titles.Add("All Sales");
                }
                else if ((product == "All") && (productClass == "All"))
                {
                    cmd.CommandText = "SELECT * FROM sales_fact_1998 LEFT JOIN store ON sales_fact_1998.store_id = store.store_id " +
                        "WHERE store.store_city = '" + city + "'";
                    //// Main title
                    chart_main.Titles.Add("Sales in " + city);
                }
                else if ((product == "All") && (city == "All"))
                {
                    cmd.CommandText = "SELECT * FROM sales_fact_1998 " +
                        "LEFT JOIN store ON sales_fact_1998.store_id = store.store_id " +
                        "LEFT JOIN product_class ON product_class.product_class_id = sales_fact_1998.product_id " +
                        "LEFT JOIN product ON product.product_class_id = product_class.product_class_id " +
                        "WHERE product.product_class_id = " + productClassID;
                    //// Main title
                    chart_main.Titles.Add("Sales by " + productClass);
                }
                else if (city == "All")
                {
                    cmd.CommandText = "SELECT * FROM sales_fact_1998 " +
                        "LEFT JOIN product_class ON product_class.product_class_id = sales_fact_1998.product_id " +
                        "LEFT JOIN product ON product.product_class_id = product_class.product_class_id " +
                        "WHERE product.product_name = '" + product + "'";
                    //// Main title
                    chart_main.Titles.Add("Sales by " + productClass + ": " + product + " in all cities");
                }
                else if (product == "All")
                {
                    cmd.CommandText = "SELECT * FROM sales_fact_1998 " +
                        "LEFT JOIN store ON sales_fact_1998.store_id = store.store_id " +
                        "LEFT JOIN product_class ON product_class.product_class_id = sales_fact_1998.product_id " +
                        "LEFT JOIN product ON product.product_class_id = product_class.product_class_id " +
                        "WHERE product.product_class_id = " + productClassID + " AND " +
                        "store.store_city = '" + city + "'";
                    //// Main title
                    chart_main.Titles.Add("Sales by " + productClass + " in " + city);
                }
                else
                {
                    cmd.CommandText = "SELECT * FROM sales_fact_1998 " +
                        "LEFT JOIN store ON sales_fact_1998.store_id = store.store_id " +
                        "LEFT JOIN product_class ON product_class.product_class_id = sales_fact_1998.product_id " +
                        "LEFT JOIN product ON product.product_class_id = product_class.product_class_id " +
                        "WHERE product.product_class_id = " + productClassID + " AND " +
                        "product.product_name = '" + product + "' AND " +
                        "store.store_city = '" + city + "'";
                    //// Main title
                    chart_main.Titles.Add("Sales by " + productClass + ": " + product + " in " + city);
                }

                // After we've created the command, lets execute it
                SqlDataReader reader = cmd.ExecuteReader();

                // We have 12 lists here. One for each month. It will hold a list of 
                // doubles, which is what we will use late to convert them all to USD depending
                // on where the product was sold and to what currency.
                List<double> JanuaryProductID = new List<double>();
                List<double> FebuaryProductID = new List<double>();
                List<double> MarchProductID = new List<double>();
                List<double> AprilProductID = new List<double>();
                List<double> MayProductID = new List<double>();
                List<double> JuneProductID = new List<double>();
                List<double> JulyProductID = new List<double>();
                List<double> AugustProductID = new List<double>();
                List<double> SeptemberProductID = new List<double>();
                List<double> OctoberProductID = new List<double>();
                List<double> NovemberProductID = new List<double>();
                List<double> DecemberProductID = new List<double>();

                // Three variables to hold the conversion of American, Mexican, and Canadian currency
                double USD = 1.00;
                double MEX = 0.10;
                double CAD = 0.67;


                // This is the processing loop that will trigger that just chooses the proper command to
                // the proper if block. Im sure there was probably a much better way to write this, but
                // given the time that I was, this was the quickest and most functional setup I could think up
                while (reader.Read())
                {
                    double price = 0;
                    if (Double.Parse(reader["currency_id"].ToString()) == 1)
                    {
                        price = Double.Parse(reader["product_id"].ToString());
                        price = price * USD;
                    }
                    else if (Double.Parse(reader["currency_id"].ToString()) == 2)
                    {
                        price = Double.Parse(reader["product_id"].ToString());
                        price = price * MEX;
                    }
                    else if (Double.Parse(reader["currency_id"].ToString()) == 3)
                    {
                        price = Double.Parse(reader["product_id"].ToString());
                        price = price * CAD;
                    }


                    // We are going to take that "time_id" number that is stored in the sales_fact_1998
                    // and parse it to the proper month. Here we calculates that January 1st, 2018 stats at
                    // 732, and contains 31 days, to 762. This follows suite to all the following months
                    string monthString = reader["time_id"].ToString();
                    int month = Int32.Parse(monthString.ToString());
                    
                    if ((month >= 732) && (month <= 762))//January
                    {
                        JanuaryProductID.Add(price);
                    }
                    else if ((month >= 763) && (month <= 790))//Febuary
                    {
                        FebuaryProductID.Add(price);
                    }
                    else if ((month >= 791) && (month <= 821))//March
                    {
                        MarchProductID.Add(price);
                    }
                    else if ((month >= 822) && (month <= 851))//April
                    {
                        AprilProductID.Add(price);
                    }
                    else if ((month >= 852) && (month <= 882))//May
                    {
                        MayProductID.Add(price);
                    }
                    else if ((month >= 883) && (month <= 912))//June
                    {
                        JuneProductID.Add(price);
                    }
                    else if ((month >= 913) && (month <= 943))//July
                    {
                        JulyProductID.Add(price);
                    }
                    else if ((month >= 944) && (month <= 974))//Aug
                    {
                        AugustProductID.Add(price);
                    }
                    else if ((month >= 975) && (month <= 1004))//Sept
                    {
                        SeptemberProductID.Add(price);
                    }
                    else if ((month >= 1005) && (month <= 1035))//Oct
                    {
                        OctoberProductID.Add(price);
                    }
                    else if ((month >= 1036) && (month <= 1065))//Nov
                    {
                        NovemberProductID.Add(price);
                    }
                    else if ((month >= 1066) && (month <= 1096))//Dec
                    {
                        DecemberProductID.Add(price);
                    }
                }
                reader.Close();
                cmd.Dispose();

                // This is where we make the calls to LoadMonthlySales() which puts the plots on the
                // graph. We used Linq to compound the total of each list when sending it to the method
                LoadMonthlySales("January", JanuaryProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("Febuary", FebuaryProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("March", MarchProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("April", AprilProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("May", MayProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("June", JuneProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("July", JulyProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("August", AugustProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("September", SeptemberProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("October", OctoberProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("November", NovemberProductID.Sum(x => Convert.ToDouble(x)));
                LoadMonthlySales("December", DecemberProductID.Sum(x => Convert.ToDouble(x)));
            }
        }

        /// <summary>
        /// This method is used to set the title of the Y axis to "month", 
        /// set the angle of the text to -90 (turning it sideways)
        /// as well as changes the intervals to 1 so that it displays each month correctly
        /// </summary>
        private void LoadXAxisTitles()
        {
            // Set the AxisX Title
            chart_main.ChartAreas["ChartArea1"].AxisX.Title = "Months - 2008";

            // Set the angle of the lettering
            chart_main.ChartAreas[0].AxisX.LabelStyle.Angle = -90;

            // Set how many columns are displayed
            chart_main.ChartAreas[0].AxisX.Interval = 1;
        }

        /// <summary>
        /// This method is used to set the title of the X axis to "sales USD $" since
        /// we convert all prices to USD $
        /// </summary>
        private void LoadYAxisTitles()
        {
            // Set the AxisX Title
            chart_main.ChartAreas["ChartArea1"].AxisY.Title = "Sales - USD $";
        }

        /// <summary>
        /// This method is the point loaded. It takes the month to insert, as well as
        /// the total sales that were calculated for that month, it then puts that point on the graph
        /// this method is called a total of 12 times each time the UpdateGraph() method is called. That is
        /// because there are 12 months to insert
        /// </summary>
        /// <param name="month">The month to insert</param>
        /// <param name="sales">The total sales for that month</param>
        private void LoadMonthlySales(string month, double sales)
        {
            if (month == "January")//January
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[0].AxisLabel = "January";
            }
            else if (month == "Febuary")//Febuary
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[1].AxisLabel = "Febuary";
            }
            else if (month == "March")//March
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[2].AxisLabel = "March";
            }
            else if (month == "April")//April
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[3].AxisLabel = "April";
            }
            else if (month == "May")//May
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[4].AxisLabel = "May";
            }
            else if (month == "June")//June
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[5].AxisLabel = "June";
            }
            else if (month == "July")//July
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[6].AxisLabel = "July";
            }
            else if (month == "August")//Aug
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[7].AxisLabel = "August";
            }
            else if (month == "September")//Sept
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[8].AxisLabel = "September";
            }
            else if (month == "October")//Oct
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[9].AxisLabel = "October";
            }
            else if (month == "November")//Nov
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[10].AxisLabel = "November";
            }
            else if (month == "December")//Dec
            {
                chart_main.Series["Test"].Points.Add(sales);
                chart_main.Series["Test"].Points[11].AxisLabel = "December";
            }
        }
    }
}
