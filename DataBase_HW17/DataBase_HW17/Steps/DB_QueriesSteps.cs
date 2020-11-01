using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;
using TechTalk.SpecFlow;

namespace DataBase_HW17.Steps
{
    [Binding]
    public class DB_QueriesSteps
    {
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=PersonsOrders;Integrated Security=True";
        string sqlExpression;
        SqlDataAdapter adapter;
        DataSet dataset;
        SqlConnection connection;
        SqlCommand sqlCommand;
        int number;

        //Field from table Order
        int id_person;
        int sum;

        //Field from table person
        int id;
        string firstName;
        string lastName;
        int age;
        string city;

        

        //insert
        [Given(@"information of a new person is ready")]
        public void GivenInformationOfANewPersonIsReady()
        {
            firstName = "Test";
            lastName = "Test2";
            age = 15;
            city = "Test";
        }
        
        [When(@"we send information about person to data bese")]
        public void WhenWeSendInformationAboutPersonToDataBese()
        {
            sqlExpression = String.Format("INSERT INTO Persons (FirstName, LastName, Age, City) VALUES ('{0}', '{1}', {2}, '{3}')",
                firstName, lastName, age, city);
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                sqlCommand = new SqlCommand(sqlExpression, connection);
                number = sqlCommand.ExecuteNonQuery();
            }
        }
        
        [Then(@"we can find information in data base which we sended")]
        public void ThenWeCanFindInformationInDataBaseWhichWeSended()
        {
            string sqlExpression = "SELECT * FROM Persons WHERE ID = (SELECT MAX(ID) FROM Persons)";
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sqlExpression, connection);

                dataset = new DataSet();
                adapter.Fill(dataset);
            }
            

            Assert.AreEqual(firstName, dataset.Tables[0].Rows[0].ItemArray[1]);
            Assert.AreEqual(lastName, dataset.Tables[0].Rows[0].ItemArray[2]);
            Assert.AreEqual(age, dataset.Tables[0].Rows[0].ItemArray[3]);
            Assert.AreEqual(city, dataset.Tables[0].Rows[0].ItemArray[4]);
        }

        //update
        [Given(@"new datas for updating and person for updating is ready")]
        public void GivenNewDatasForUpdatingAndPersonForUpdatingIsReady()
        {
            firstName = "UpdateName";
            lastName = "UprateLastName";
            age = 99;
            city = "Vavilon";
        }

        [When(@"I updating information aboun person")]
        public void WhenIUpdatingInformationAbounPerson()
        {
            string sqlExpression = String.Format("UPDATE Persons SET FirstName={0}, LastName={1}, Age={2}, City={3}  WHERE Id=(SELECT MAX(Id) FROM Persons)",
                            firstName, lastName, age, city);

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                sqlCommand = new SqlCommand(sqlExpression, connection);
                //number = sqlCommand.ExecuteNonQuery();
            }
        }

        [Then(@"Information about person has been edited")]
        public void ThenInformationAboutPersonHasBeenEdited()
        {
            string sqlExpression = String.Format("SELECT FirstName, LastName, Age, City FROM Persons WHERE ID=(SELECT MAX(ID) FROM Persons)");
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sqlExpression, connection);

                dataset = new DataSet();
                adapter.Fill(dataset);
            }
            Assert.AreEqual(firstName, dataset.Tables[0].Rows[0].ItemArray[0]);
            Assert.AreEqual(lastName, dataset.Tables[0].Rows[0].ItemArray[1]);
            Assert.AreEqual(age, dataset.Tables[0].Rows[0].ItemArray[2]);
            Assert.AreEqual(city, dataset.Tables[0].Rows[0].ItemArray[3]);
        }

        //add order
        [Given(@"datas for order and person is ready")]
        public void GivenDatasForOrderAndPersonIsReady()
        {
            id_person = 1;
            sum = 777777;
        }

        [When(@"we adding order for person")]
        public void WhenWeAddingOrderForPerson()
        {
            sqlExpression = String.Format("INSERT INTO Orders (ID, SUM_order) VALUES ('{0}', '{1}')",
                id_person, sum);
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                sqlCommand = new SqlCommand(sqlExpression, connection);
                number = sqlCommand.ExecuteNonQuery();
            }
        }

        [Then(@"Order has been created for person")]
        public void ThenOrderHasBeenCreatedForPerson()
        {
            string sqlExpression = String.Format("SELECT TOP(1) Persons.ID, Orders.ID_order, Orders.SUM_order " +
                "FROM Orders LEFT JOIN Persons ON Persons.ID = Orders.ID " +
                "WHERE Persons.ID = 1 ORDER BY Orders.ID_order DESC;");

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sqlExpression, connection);

                dataset = new DataSet();
                adapter.Fill(dataset);
            }

            Assert.AreEqual(id_person, dataset.Tables[0].Rows[0].ItemArray[0]);
            Assert.AreEqual(sum, dataset.Tables[0].Rows[0].ItemArray[2]);
        }

        //delete person

        [When(@"we select a person for delete")]
        public void WhenWeSelectAPersonForDelete()
        {
            string sqlExpression = "SELECT * FROM Persons WHERE ID = (SELECT MAX(ID) FROM Persons)";
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sqlExpression, connection);

                dataset = new DataSet();
                adapter.Fill(dataset);
            }

            id = (int)dataset.Tables[0].Rows[0].ItemArray[0];
            firstName = (string)dataset.Tables[0].Rows[0].ItemArray[1];
            lastName = (string)dataset.Tables[0].Rows[0].ItemArray[2];
            age = (int)dataset.Tables[0].Rows[0].ItemArray[3];
            //city = (string)dataset.Tables[0].Rows[0].ItemArray[3];

            sqlExpression = String.Format("DELETE FROM Orders WHERE Id={0}", id);
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                sqlCommand = new SqlCommand(sqlExpression, connection);
                number = sqlCommand.ExecuteNonQuery();
            }
        }

        [Then(@"person has been deleted")]
        public void ThenPersonHasBeenDeleted()
        {
            string sqlExpression = "SELECT * From Orders WHERE Id = (SELECT MAX(Id) FROM Orders)";
            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(sqlExpression, connection);

                dataset = new DataSet();
                adapter.Fill(dataset);
            }

            Assert.AreNotEqual(id, dataset.Tables[0].Rows[0].ItemArray[0]);
            Assert.AreEqual(0, number);
        }
    }
}
