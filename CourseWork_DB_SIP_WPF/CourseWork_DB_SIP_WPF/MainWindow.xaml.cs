using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
namespace CourseWork_DB_SIP_WPF
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=DESKTOP-GGI31RV\\MSSQLSERVERNEW;Initial Catalog=SIP;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }
        //Завантаження даних в таблиці та оновлення даних в них
        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string homeQuery = "SELECT * FROM InsuranceHome";
                    SqlDataAdapter homeAdapter = new SqlDataAdapter(homeQuery, connection);
                    DataTable homeTable = new DataTable();
                    homeAdapter.Fill(homeTable);
                    HomeTBL1.ItemsSource = homeTable.DefaultView;

                    string lifeQuery = "SELECT * FROM InsuranceLife";
                    SqlDataAdapter lifeAdapter = new SqlDataAdapter(lifeQuery, connection);
                    DataTable lifeTable = new DataTable();
                    lifeAdapter.Fill(lifeTable);
                    LifeTBL1.ItemsSource = lifeTable.DefaultView;

                    string autoQuery = "SELECT * FROM InsuranceAuto";
                    SqlDataAdapter autoAdapter = new SqlDataAdapter(autoQuery, connection);
                    DataTable autoTable = new DataTable();
                    autoAdapter.Fill(autoTable);
                    AutoTBL1.ItemsSource = autoTable.DefaultView;

                    string agentsQuery = "SELECT * FROM Agents";
                    SqlDataAdapter agentsAdapter = new SqlDataAdapter(agentsQuery, connection);
                    DataTable agentsTable = new DataTable();
                    agentsAdapter.Fill(agentsTable);
                    AgentTBL1.ItemsSource = agentsTable.DefaultView;

                    string policiesQuery = "SELECT * FROM Policies";
                    SqlDataAdapter policiesAdapter = new SqlDataAdapter(policiesQuery, connection);
                    DataTable policiesTable = new DataTable();
                    policiesAdapter.Fill(policiesTable);

                    PoliciesTBL1.ItemsSource = policiesTable.DefaultView;
                    PoliciesTBL2.ItemsSource = policiesTable.DefaultView;
                    PoliciesTBL3.ItemsSource = policiesTable.DefaultView;

                    string clientsQuery = "SELECT * FROM Clients";
                    SqlDataAdapter clientsAdapter = new SqlDataAdapter(clientsQuery, connection);
                    DataTable clientsTable = new DataTable();
                    clientsAdapter.Fill(clientsTable);
                    ClientTBL1.ItemsSource = clientsTable.DefaultView;

                    string eventQuery = "SELECT * FROM InsuranceEvent";
                    SqlDataAdapter eventAdapter = new SqlDataAdapter(eventQuery, connection);
                    DataTable eventTable = new DataTable();
                    eventAdapter.Fill(eventTable);
                    InsuranceEventTBL.ItemsSource = eventTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка завантаження даних: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }



        // Фільтр полісів по типам
        private void InsuranceAutoRB_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilterByType("Страхування автомобіля");
        }
        private void InsuranceHomeRB_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilterByType("Страхування житла");
        }
        private void InsuranceLifeRB_Checked(object sender, RoutedEventArgs e)
        {
            ApplyFilterByType("Страхування життя");
        }
        private void ClearFilterBTN_Click(object sender, RoutedEventArgs e)
        {
            InsuranceAutoRB.IsChecked = false;
            InsuranceLifeRB.IsChecked = false;
            InsuranceHomeRB.IsChecked = false;

            ApplyFilterByType("");
            ApplyFilterByDates(null, null);
            LoadData();
        }
        private void ApplyFilterByType(string filter)
        {
            string queryPolicies = "SELECT * FROM Policies";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (!string.IsNullOrEmpty(filter))
                    {
                        queryPolicies += " WHERE PoliciesType = @Filter";

                    }

                    SqlDataAdapter adapterPolicies = new SqlDataAdapter(queryPolicies, connection);


                    if (!string.IsNullOrEmpty(filter))
                    {
                        adapterPolicies.SelectCommand.Parameters.AddWithValue("@Filter", filter);
                    }

                    DataTable dataTablePolicies = new DataTable();
                    adapterPolicies.Fill(dataTablePolicies);
                    PoliciesTBL1.ItemsSource = dataTablePolicies.DefaultView;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка завантаження даних: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Фільтр полісів за датами
        private void DatePicker1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterByDates(DatePicker1.SelectedDate, DatePicker2.SelectedDate);
        }
        private void DatePicker2_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilterByDates(DatePicker1.SelectedDate, DatePicker2.SelectedDate);
        }
        private void ClearFilterDataBTN_Click(object sender, RoutedEventArgs e)
        {
            DatePicker1.SelectedDate = null;
            DatePicker2.SelectedDate = null;

            ApplyFilterByType("");
            ApplyFilterByDates(null, null);
            LoadData();
        }
        private void ApplyFilterByDates(DateTime? startDate, DateTime? endDate)
        {
            string queryPolicies = "SELECT * FROM Policies WHERE 1=1";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (startDate != null && endDate != null)
                    {
                        queryPolicies += " AND StartDate >= @StartDate AND EndDate <= @EndDate";
                    }

                    SqlDataAdapter adapterPolicies = new SqlDataAdapter(queryPolicies, connection);

                    if (startDate != null && endDate != null)
                    {
                        adapterPolicies.SelectCommand.Parameters.AddWithValue("@StartDate", startDate);
                        adapterPolicies.SelectCommand.Parameters.AddWithValue("@EndDate", endDate);
                    }

                    DataTable dataTablePolicies = new DataTable();
                    adapterPolicies.Fill(dataTablePolicies);
                    PoliciesTBL1.ItemsSource = dataTablePolicies.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка завантаження даних: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }




        // Основні взаємодії з табл агентів
        private void SaveAgentBTN_Click(object sender, RoutedEventArgs e)
        {
            string agentFiName = AgentFiTB.Text;
            string agentSeName = AgentSeTB.Text;
            string agentFaName = AgentFaTB.Text;
            string agentPhone = AgentPhoTB.Text;
            string agentPost = AgentPostTB.Text;
            string agentID = AgentIDTB.Text;

            if (string.IsNullOrWhiteSpace(agentFiName) || string.IsNullOrWhiteSpace(agentSeName) || string.IsNullOrWhiteSpace(agentFaName) ||
                string.IsNullOrWhiteSpace(agentPhone) || string.IsNullOrWhiteSpace(agentPost) || string.IsNullOrWhiteSpace(agentID))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.");
                return;
            }

            if (!Regex.IsMatch(agentPhone, @"^\+\d{3}\s\d{2}\s\d{3}\s\d{4}$"))
            {
                MessageBox.Show("Невірний формат номера телефону. Використовуйте формат: +XXX XX XXX XXXX");
                return;
            }

            if (!IsUpperCase(agentFiName) || !IsUpperCase(agentSeName) || !IsUpperCase(agentFaName))
            {
                MessageBox.Show("Ім'я, по-батькові або прізвище мають бути написані з великої літери.");
                return;
            }

            if (IsAgentIDExists(agentID) || IsAgentPhoneExists(agentPhone))
            {
                MessageBox.Show("Агент з таким ID або номером телефона вже існує!");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO Agents (IdAgent, AgentFiName, AgentSeName, AgentFaName, AgentPhone, AgentPost) " +
                                   "VALUES (@IdAgent, @AgentFiName, @AgentSeName, @AgentFaName, @AgentPhone, @AgentPost)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdAgent", agentID);
                    cmd.Parameters.AddWithValue("@AgentFiName", agentFiName);
                    cmd.Parameters.AddWithValue("@AgentSeName", agentSeName);
                    cmd.Parameters.AddWithValue("@AgentFaName", agentFaName);
                    cmd.Parameters.AddWithValue("@AgentPhone", agentPhone);
                    cmd.Parameters.AddWithValue("@AgentPost", agentPost);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Агент успішно доданий!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні агента: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            LoadData();
        }
        private void AsseptChangeAgentBTN_Click(object sender, RoutedEventArgs e)
        {
            string agentID = AgentIDTB.Text;
            string agentFiName = AgentFiTB.Text;
            string agentSeName = AgentSeTB.Text;
            string agentFaName = AgentFaTB.Text;
            string agentPhone = AgentPhoTB.Text;
            string agentPost = AgentPostTB.Text;

            if (string.IsNullOrWhiteSpace(agentFiName) || string.IsNullOrWhiteSpace(agentSeName) || string.IsNullOrWhiteSpace(agentFaName) ||
                string.IsNullOrWhiteSpace(agentPhone) || string.IsNullOrWhiteSpace(agentPost) || string.IsNullOrWhiteSpace(agentID))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.");
                return;
            }

            if (!Regex.IsMatch(agentPhone, @"^\+\d{3}\s\d{2}\s\d{3}\s\d{4}$"))
            {
                MessageBox.Show("Невірний формат номера телефону. Використовуйте формат: +XXX XX XXX XXXX");
                return;
            }

            if (!IsUpperCase(agentFiName) || !IsUpperCase(agentSeName) || !IsUpperCase(agentFaName))
            {
                MessageBox.Show("Ім'я, по-батькові або прізвище мають бути написані з великої літери.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Agents SET AgentFiName = @AgentFiName, AgentSeName = @AgentSeName, " +
                                   "AgentFaName = @AgentFaName, AgentPhone = @AgentPhone, AgentPost = @AgentPost " +
                                   "WHERE IdAgent = @AgentID";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@AgentFiName", agentFiName);
                    cmd.Parameters.AddWithValue("@AgentSeName", agentSeName);
                    cmd.Parameters.AddWithValue("@AgentFaName", agentFaName);
                    cmd.Parameters.AddWithValue("@AgentPhone", agentPhone);
                    cmd.Parameters.AddWithValue("@AgentPost", agentPost);
                    cmd.Parameters.AddWithValue("@AgentID", agentID);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Дані агента успешно змінені!");
                        FilterAgentDG();
                        ClearAgentFields();
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося змінити дані агента.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при зміні даних агента: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void DeleteAgentBTN_Click(object sender, RoutedEventArgs e)
        {
            string agentIDToDelete = AgentIDTB.Text;
            if (string.IsNullOrWhiteSpace(agentIDToDelete))
            {
                MessageBox.Show("Введіть ID агента для видалення.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете видалити агента з цим ID?", "Видалення агента", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();

                        string deletePoliciesQuery = "DELETE FROM Policies WHERE IdAgent = @AgentID";
                        SqlCommand deletePoliciesCmd = new SqlCommand(deletePoliciesQuery, connection, transaction);
                        deletePoliciesCmd.Parameters.AddWithValue("@AgentID", agentIDToDelete);
                        deletePoliciesCmd.ExecuteNonQuery();

                        string deleteAgentQuery = "DELETE FROM Agents WHERE IdAgent = @AgentID";
                        SqlCommand deleteAgentCmd = new SqlCommand(deleteAgentQuery, connection, transaction);
                        deleteAgentCmd.Parameters.AddWithValue("@AgentID", agentIDToDelete);
                        int rowsAffected = deleteAgentCmd.ExecuteNonQuery();

                        transaction.Commit();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Агент і пов'язані з ним поліси були видалені.");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Агент з таким ID не знайдений.");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Помилка при видаленні агента та його полісів " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        private bool IsUpperCase(string input)
        {
            return char.IsUpper(input[0]);
        }
        private bool IsAgentIDExists(string agentID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Agents WHERE IdAgent = @AgentID", connection);
                command.Parameters.AddWithValue("@AgentID", agentID);

                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }
        private bool IsAgentPhoneExists(string agentPhone)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Agents WHERE AgentPhone = @AgentPhone", connection);
                command.Parameters.AddWithValue("@AgentPhone", agentPhone);

                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
        }
        private void AgentTBL2_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (AgentTBL1.SelectedItem != null && AgentTBL1.SelectedItem is DataRowView)
            {
                DataRowView row = (DataRowView)AgentTBL1.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Хочете змінити дані цього робітника?", "Зміна данних", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    AgentIDTB.Text = row["IdAgent"].ToString();
                    AgentFiTB.Text = row["AgentFiName"].ToString();
                    AgentSeTB.Text = row["AgentSeName"].ToString();
                    AgentFaTB.Text = row["AgentFaName"].ToString();
                    AgentPhoTB.Text = row["AgentPhone"].ToString();
                    AgentPostTB.Text = row["AgentPost"].ToString();
                }
            }
            else
            {
                MessageBox.Show("Тут ще немає робітника.");
            }
        }

        // Фільтр даних агентів по прізвищу або номеру
        private void FilterAgentDG()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Agents";
                    if (AgentPhoneRB.IsChecked == true)
                    {
                        query += " WHERE AgentPhone LIKE @SearchText";
                    }
                    else if (AgentSeRB.IsChecked == true)
                    {
                        query += " WHERE AgentSeName LIKE @SearchText";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + SeachAgentTB.Text + "%");
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    AgentTBL1.ItemsSource = dataTable.DefaultView;
                    FilterPoliciesAGEDG();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка оновлень данних агентів: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void FilterPoliciesAGEDG()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Policies.* FROM Policies INNER JOIN Agents ON Policies.IdAgent = Agents.IdAgent";
                    if (AgentPhoneRB.IsChecked == true)
                    {
                        query += " WHERE Agents.AgentPhone LIKE @SearchText";
                    }
                    else if (AgentSeRB.IsChecked == true)
                    {
                        query += " WHERE Agents.AgentSeName LIKE @SearchText";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + SeachAgentTB.Text + "%");
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    PoliciesTBL2.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка оновлення данних полісів: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Фільтр агентів рб і тб
        private void AgentSeRB_Checked(object sender, RoutedEventArgs e)
        {
            FilterAgentDG();
            FilterPoliciesAGEDG();
        }
        private void AgentPhoneRB_Checked(object sender, RoutedEventArgs e)
        {
            FilterAgentDG();
            FilterPoliciesAGEDG();
        }
        private void ClearFilterAgentBTN_Click(object sender, RoutedEventArgs e)
        {
            AgentPhoneRB.IsChecked = false;
            AgentSeRB.IsChecked = false;
            FilterAgentDG();
            FilterPoliciesAGEDG();
        }
        private void SeachAgentTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FilterAgentDG();
            FilterPoliciesAGEDG();
        }
        private void ClearAgentFields()
        {
            AgentIDTB.Text = "";
            AgentFiTB.Text = "";
            AgentSeTB.Text = "";
            AgentFaTB.Text = "";
            AgentPhoTB.Text = "";
            AgentPostTB.Text = "";
        }




        // Основні взаємодії з табл клієнтів
        private void SaveClientBTN_Click(object sender, RoutedEventArgs e)
        {
            string idClients = ClientIDTB.Text;
            string clientFiName = ClientFiTB.Text;
            string clientSeName = ClientSeTB.Text;
            string clientFaName = ClientFaTB.Text;
            string clientPhone = ClientPhoTB.Text;
            string clientAddress = ClientAddressTB.Text;

            if (!Regex.IsMatch(clientPhone, @"^\+\d{3}\s\d{2}\s\d{3}\s\d{4}$"))
            {
                MessageBox.Show("Невірний формат номера телефону. Використовуйте формат: +XXX XX XXX XXXX");
                return;
            }
            if (!Regex.IsMatch(clientAddress, @"вул\.\s\w+,\s\d+,\s\d+"))
            {
                MessageBox.Show("Невірний формат адреса. Використовуйте формат: вул. XXXX, NN, YYY");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string checkExistingQuery = "SELECT COUNT(*) FROM Clients WHERE IdClients = @IdClients OR ClientPhone = @ClientPhone";
                    SqlCommand checkExistingCmd = new SqlCommand(checkExistingQuery, connection);
                    checkExistingCmd.Parameters.AddWithValue("@IdClients", idClients);
                    checkExistingCmd.Parameters.AddWithValue("@ClientPhone", clientPhone);

                    int existingCount = (int)checkExistingCmd.ExecuteScalar();

                    if (existingCount > 0)
                    {
                        MessageBox.Show("Клієнт з таким же ID або номером телефона вже існує! Змініть дані.");
                        return;
                    }
                    string insertQuery = "INSERT INTO Clients (IdClients, ClientFiName, ClientSeName, ClientFaName, ClientPhone, ClientAddress) " +
                                         "VALUES (@IdClients, @ClientFiName, @ClientSeName, @ClientFaName, @ClientPhone, @ClientAddress)";

                    SqlCommand cmd = new SqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@IdClients", idClients);
                    cmd.Parameters.AddWithValue("@ClientFiName", clientFiName);
                    cmd.Parameters.AddWithValue("@ClientSeName", clientSeName);
                    cmd.Parameters.AddWithValue("@ClientFaName", clientFaName);
                    cmd.Parameters.AddWithValue("@ClientPhone", clientPhone);
                    cmd.Parameters.AddWithValue("@ClientAddress", clientAddress);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Клієнт успішно доданий!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні клієнта: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                FilterClientDG();
                ClearClientFields();
            }
        }
        private void AsseptChangeClientBTN_Click(object sender, RoutedEventArgs e)
        {
            string IdClients = ClientIDTB.Text;
            string clientFiName = ClientFiTB.Text;
            string clientSeName = ClientSeTB.Text;
            string clientFaName = ClientFaTB.Text;
            string clientPhone = ClientPhoTB.Text;
            string clientAddress = ClientAddressTB.Text;

            if (!Regex.IsMatch(clientPhone, @"^\+\d{3}\s\d{2}\s\d{3}\s\d{4}$"))
            {
                MessageBox.Show("Невірний формат номера телефону. Використовуйте формат: +XXX XX XXX XXXX");
                return;
            }
            if (!Regex.IsMatch(clientAddress, @"^вул\.\s\w+,\s\d+,\s\d+$"))
            {
                MessageBox.Show("Невірний формат адреса. Використовуйте формат: вул. XXXX, NN, YYY");
                return;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "UPDATE Clients SET ClientFiName = @ClientFiName, ClientSeName = @ClientSeName, " +
                                   "ClientFaName = @ClientFaName, ClientPhone = @ClientPhone, ClientAddress = @ClientAddress " +
                                   "WHERE IdClients = @IdClients";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@IdClients", IdClients);
                    cmd.Parameters.AddWithValue("@ClientFiName", clientFiName);
                    cmd.Parameters.AddWithValue("@ClientSeName", clientSeName);
                    cmd.Parameters.AddWithValue("@ClientFaName", clientFaName);
                    cmd.Parameters.AddWithValue("@ClientPhone", clientPhone);
                    cmd.Parameters.AddWithValue("@ClientAddress", clientAddress);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Дані клієнта успішно змінені!");
                        FilterClientDG();
                        ClearClientFields();
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося змінити дані клієнта.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при зміні даних клієнта: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void DeleteClientBTN_Click(object sender, RoutedEventArgs e)
        {
            string clientIDToDelete = ClientIDTB.Text;
            if (string.IsNullOrEmpty(clientIDToDelete))
            {
                MessageBox.Show("Введіть ID клієнта для видалення.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете видалити цього клієнта?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlTransaction transaction = null;

                    try
                    {
                        connection.Open();
                        transaction = connection.BeginTransaction();

                        string deletePoliciesQuery = "DELETE FROM Policies WHERE IdClients = @IdClients";
                        SqlCommand deletePoliciesCmd = new SqlCommand(deletePoliciesQuery, connection, transaction);
                        deletePoliciesCmd.Parameters.AddWithValue("@IdClients", clientIDToDelete);
                        deletePoliciesCmd.ExecuteNonQuery();

                        string deleteClientQuery = "DELETE FROM Clients WHERE IdClients = @IdClients";
                        SqlCommand deleteClientCmd = new SqlCommand(deleteClientQuery, connection, transaction);
                        deleteClientCmd.Parameters.AddWithValue("@IdClients", clientIDToDelete);
                        int rowsAffected = deleteClientCmd.ExecuteNonQuery();

                        transaction.Commit();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Клієнт і пов'язані з ним поліси були видалені.");
                            FilterClientDG();
                        }
                        else
                        {
                            MessageBox.Show("Клієнт з таким ID не знайдений.");
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction?.Rollback();
                        MessageBox.Show("Помилка при видаленні клієнта та його полісів " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
        private void ClientTBL_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ClientTBL1.SelectedItem != null && ClientTBL1.SelectedItem is DataRowView)
            {
                DataRowView row = (DataRowView)ClientTBL1.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Хочете змінити дані цього клієнта?", "Зміна данних", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    ClientIDTB.Text = row["IdClients"].ToString();
                    ClientFiTB.Text = row["ClientFiName"].ToString();
                    ClientSeTB.Text = row["ClientSeName"].ToString();
                    ClientFaTB.Text = row["ClientFaName"].ToString();
                    ClientPhoTB.Text = row["ClientPhone"].ToString();
                    ClientAddressTB.Text = row["ClientAddress"].ToString();
                }
            }
            else
            {
                MessageBox.Show("Тут ще немає клієнта.");
            }
        }

        // Фільтр даних клієнтів по прізвищу або номеру
        private void FilterClientDG()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM Clients";
                    if (ClientPhoneRB.IsChecked == true)
                    {
                        query += " WHERE ClientPhone LIKE @SearchText";
                    }
                    else if (ClientSeRB.IsChecked == true)
                    {
                        query += " WHERE ClientSeName LIKE @SearchText";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + SeachClientTB.Text + "%");

                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    ClientTBL1.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка оновлень данних клієнтів: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void FilterPoliciesСLIDG()
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT Policies.* FROM Policies INNER JOIN Clients ON Policies.IdClients = Clients.IdClients";
                    if (ClientPhoneRB.IsChecked == true)
                    {
                        query += " WHERE Clients.ClientPhone LIKE @SearchText";
                    }
                    else if (ClientSeRB.IsChecked == true)
                    {
                        query += " WHERE Clients.ClientSeName LIKE @SearchText";
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + SeachClientTB.Text + "%");
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    PoliciesTBL3.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка оновлення данних полісів: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        // Фільтр клієнтів рб і тб
        private void ClientSeRB_Checked(object sender, RoutedEventArgs e)
        {
            FilterClientDG();
            FilterPoliciesСLIDG();
        }
        private void ClientPhoneRB_Checked(object sender, RoutedEventArgs e)
        {
            FilterClientDG();
            FilterPoliciesСLIDG();
        }
        private void ClearFilterClientBTN_Click(object sender, RoutedEventArgs e)
        {
            ClientPhoneRB.IsChecked = false;
            ClientSeRB.IsChecked = false;
            LoadData();
        }
        private void SeachClientTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            FilterClientDG();
            FilterPoliciesСLIDG();
        }
        private void ClearClientFields()
        {
            ClientFiTB.Text = "";
            ClientSeTB.Text = "";
            ClientFaTB.Text = "";
            ClientPhoTB.Text = "";
            ClientAddressTB.Text = "";
        }




        // Взаємодія з табл Policies
        private void ChangePolBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdPoliciesNewTB.Text) && !string.IsNullOrEmpty(SummaPolNewTB.Text)
                && !string.IsNullOrEmpty(IdAgentNewTB.Text) && !string.IsNullOrEmpty(IdClientNewTB.Text)
                && DateStartDP.SelectedDate != null && DateEndDP.SelectedDate != null)
            {
                int idPolicies, idAgent, idClient;
                decimal summaPol;
                DateTime startDate, endDate;

                if (!int.TryParse(IdPoliciesNewTB.Text, out idPolicies) || !decimal.TryParse(SummaPolNewTB.Text, out summaPol)
                    || !int.TryParse(IdAgentNewTB.Text, out idAgent) || !int.TryParse(IdClientNewTB.Text, out idClient)
                    || !DateTime.TryParse(DateStartDP.Text, out startDate) || !DateTime.TryParse(DateEndDP.Text, out endDate))
                {
                    MessageBox.Show("Невірний формат введених даних.");
                    return;
                }
                if (summaPol <= 0)
                {
                    MessageBox.Show("Сума поліса повина бути додатковим числом.");
                    return;
                }
                if (!AgentExists(idAgent))
                {
                    MessageBox.Show("Агента з таким ID не існує.");
                    return;
                }
                if (!ClientExists(idClient))
                {
                    MessageBox.Show("Клієнта з таким ID не існує.");
                    return;
                }
                if (startDate > endDate)
                {
                    MessageBox.Show("Дата початку не може бути пізніше за дату завершення.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE Policies SET StartDate = @StartDate, EndDate = @EndDate, SummaPol = @SummaPol, IdAgent = @IdAgent, IdClients = @IdClients WHERE IdPolicies = @IdPolicies";

                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@StartDate", startDate);
                        cmd.Parameters.AddWithValue("@EndDate", endDate);
                        cmd.Parameters.AddWithValue("@SummaPol", summaPol);
                        cmd.Parameters.AddWithValue("@IdAgent", idAgent);
                        cmd.Parameters.AddWithValue("@IdClients", idClient);
                        cmd.Parameters.AddWithValue("@IdPolicies", idPolicies);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string updateStatusQuery = "UPDATE Policies SET PolicyStatus = " +
                                                       "CASE " +
                                                       "WHEN GETDATE() < StartDate THEN 'Неактивний' " +
                                                       "WHEN GETDATE() BETWEEN StartDate AND EndDate THEN 'Активний' " +
                                                       "ELSE 'Прострочений' " +
                                                       "END " +
                                                       "WHERE IdPolicies = @IdPolicies";

                            SqlCommand updateStatusCmd = new SqlCommand(updateStatusQuery, connection);
                            updateStatusCmd.Parameters.AddWithValue("@IdPolicies", idPolicies);
                            updateStatusCmd.ExecuteNonQuery();

                            MessageBox.Show("Дані поліса успішно змінені!");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Не вдалося змінити поліс.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при зміні даних поліса: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Заповніть усі поля для зміни даних.");
            }
        }
        private bool AgentExists(int agentId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Agents WHERE IdAgent = @AgentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AgentId", agentId);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private bool ClientExists(int clientId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Clients WHERE IdClients = @ClientId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClientId", clientId);
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        private void DeletePolBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdPoliciesNewTB.Text) && int.TryParse(IdPoliciesNewTB.Text, out int idPolicies))
            {
                MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете видалити цей поліс?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    int errorCount = 0;

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            SqlCommand checkAutoCommand = new SqlCommand("SELECT IdPolicies FROM InsuranceAuto WHERE IdPolicies = @IdPolicies;", connection);
                            checkAutoCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                            object autoResult = checkAutoCommand.ExecuteScalar();

                            if (autoResult != null)
                            {
                                SqlCommand deleteAutoCommand = new SqlCommand("DELETE FROM InsuranceAuto WHERE IdPolicies = @IdPolicies;", connection);
                                deleteAutoCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                                deleteAutoCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                errorCount++;
                            }

                            SqlCommand checkLifeCommand = new SqlCommand("SELECT IdPolicies FROM InsuranceLife WHERE IdPolicies = @IdPolicies;", connection);
                            checkLifeCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                            object lifeResult = checkLifeCommand.ExecuteScalar();

                            if (lifeResult != null)
                            {
                                SqlCommand deleteLifeCommand = new SqlCommand("DELETE FROM InsuranceLife WHERE IdPolicies = @IdPolicies;", connection);
                                deleteLifeCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                                deleteLifeCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                errorCount++;
                            }

                            SqlCommand checkHomeCommand = new SqlCommand("SELECT IdPolicies FROM InsuranceHome WHERE IdPolicies = @IdPolicies;", connection);
                            checkHomeCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                            object homeResult = checkHomeCommand.ExecuteScalar();

                            if (homeResult != null)
                            {
                                SqlCommand deleteHomeCommand = new SqlCommand("DELETE FROM InsuranceHome WHERE IdPolicies = @IdPolicies;", connection);
                                deleteHomeCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                                deleteHomeCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                errorCount++;
                            }

                            SqlCommand deletePoliciesCommand = new SqlCommand("DELETE FROM Policies WHERE IdPolicies = @IdPolicies;", connection);
                            deletePoliciesCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                            deletePoliciesCommand.ExecuteNonQuery();

                            if (errorCount == 3)
                            {
                                MessageBox.Show("Не вдалося видалити поліс.");
                            }
                            else
                            {
                                MessageBox.Show("Дані успішно видалені.");
                                LoadData();
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Помилка видалення даних: " + ex.Message);
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Введіть Id поліса для видалення.");
            }
        }
        private void PoliciesTBL4_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PoliciesTBL1.SelectedItem != null && PoliciesTBL1.SelectedItem is DataRowView)
            {
                DataRowView row = (DataRowView)PoliciesTBL1.SelectedItem;
                if (row["IdPolicies"] != DBNull.Value && row["SummaPol"] != DBNull.Value &&
                    row["IdAgent"] != DBNull.Value && row["IdClients"] != DBNull.Value &&
                    row["StartDate"] != DBNull.Value && row["EndDate"] != DBNull.Value)
                {
                    MessageBoxResult result = MessageBox.Show("Хочете змінити дані цього поліса?", "Зміна данних", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        IdPoliciesNewTB.Text = row["IdPolicies"].ToString();
                        SummaPolNewTB.Text = row["SummaPol"].ToString();
                        IdAgentNewTB.Text = row["IdAgent"].ToString();
                        IdClientNewTB.Text = row["IdClients"].ToString();
                        DateStartDP.SelectedDate = (DateTime)row["StartDate"];
                        DateEndDP.SelectedDate = (DateTime)row["EndDate"];
                    }
                }
                else
                {
                    MessageBox.Show("Тут ще немає поліса.");
                }
            }
        }

        // Взаємодія з табл InsuranceAuto
        private void AddPoliciesAutoBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput())
            {
                int idPolicies, idAgent, idClient;
                decimal summaPol, autoCost;
                DateTime startDate, endDate;

                if (!int.TryParse(IdPoliciesNewTB.Text, out idPolicies) ||
                    !decimal.TryParse(SummaPolNewTB.Text, out summaPol) ||
                    !int.TryParse(IdAgentNewTB.Text, out idAgent) ||
                    !int.TryParse(IdClientNewTB.Text, out idClient) ||
                    !DateTime.TryParse(DateStartDP.Text, out startDate) ||
                    !DateTime.TryParse(DateEndDP.Text, out endDate) ||
                    !decimal.TryParse(AutoCostTB.Text, out autoCost))
                {
                    MessageBox.Show("Невірний формат введених даних.");
                    return;
                }

                string autoNumber = AutoNumberTB.Text;
                int autoYear;

                if (!int.TryParse(AutoYearTB.Text, out autoYear))
                {
                    MessageBox.Show("Невірний формат року виробництва авто.");
                    return;
                }
                if (autoYear < 1970 || autoYear > DateTime.Now.Year)
                {
                    MessageBox.Show("Рік виробництва автомобіля має бути після 1970 і не більше поточного року.");
                    return;
                }
                if (startDate > endDate)
                {
                    MessageBox.Show("Дата початку не може бути пізніше за дату завершення.");
                    return;
                }
                if (summaPol <= 0 || autoCost <= 0)
                {
                    MessageBox.Show("Сумма поліса та вартість авто мають бути положительними числами.");
                    return;
                }
                string status;
                DateTime currentDate = DateTime.Now;
                if (currentDate < startDate)
                {
                    status = "Неактивний";
                }
                else if (currentDate >= startDate && currentDate <= endDate)
                {
                    status = "Активний";
                }
                else
                {
                    status = "Прострочений";
                }
                string insertPoliciesQuery = "INSERT INTO Policies (IdPolicies, StartDate, EndDate, SummaPol, PoliciesType, IdClients, IdAgent, PolicyStatus) " +
                                             "VALUES (@IdPolicies, @StartDate, @EndDate, @SummaPol, @PoliciesType, @IdClients, @IdAgent, @PolicyStatus)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string checkPolicyQuery = "SELECT COUNT(*) FROM Policies WHERE IdPolicies = @IdPolicies";
                        SqlCommand checkPolicyCommand = new SqlCommand(checkPolicyQuery, connection);
                        checkPolicyCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);

                        int existingPoliciesCount = (int)checkPolicyCommand.ExecuteScalar();

                        if (existingPoliciesCount > 0)
                        {
                            MessageBox.Show("Поліс з таким ID вже існує.");
                            return;
                        }

                        SqlCommand insertPoliciesCommand = new SqlCommand(insertPoliciesQuery, connection);
                        insertPoliciesCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                        insertPoliciesCommand.Parameters.AddWithValue("@StartDate", startDate);
                        insertPoliciesCommand.Parameters.AddWithValue("@EndDate", endDate);
                        insertPoliciesCommand.Parameters.AddWithValue("@SummaPol", summaPol);
                        insertPoliciesCommand.Parameters.AddWithValue("@PoliciesType", "Страхування автомобіля");
                        insertPoliciesCommand.Parameters.AddWithValue("@IdClients", idClient);
                        insertPoliciesCommand.Parameters.AddWithValue("@IdAgent", idAgent);
                        insertPoliciesCommand.Parameters.AddWithValue("@PolicyStatus", status);

                        int rowsAffected = insertPoliciesCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            string insertAutoQuery = "INSERT INTO InsuranceAuto (IdPolicies, AutoNumber, AutoYear, AutoCost) VALUES (@IdPolicies, @AutoNumber, @AutoYear, @AutoCost)";
                            SqlCommand insertAutoCommand = new SqlCommand(insertAutoQuery, connection);
                            insertAutoCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                            insertAutoCommand.Parameters.AddWithValue("@AutoNumber", autoNumber);
                            insertAutoCommand.Parameters.AddWithValue("@AutoYear", autoYear);
                            insertAutoCommand.Parameters.AddWithValue("@AutoCost", autoCost);

                            int rowsAffectedAuto = insertAutoCommand.ExecuteNonQuery();

                            if (rowsAffectedAuto > 0)
                            {
                                MessageBox.Show("Поліс та інформація про авто успішно додані.");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Не вдалося додати інформацію про авто.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Не вдалося додати поліс.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при додаванні поліса та інформації про авто: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Перевірте правильність введення даних.");
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrEmpty(IdPoliciesNewTB.Text) ||
                string.IsNullOrEmpty(SummaPolNewTB.Text) ||
                string.IsNullOrEmpty(IdAgentNewTB.Text) ||
                string.IsNullOrEmpty(IdClientNewTB.Text) ||
                DateStartDP.SelectedDate == null ||
                DateEndDP.SelectedDate == null ||
                string.IsNullOrEmpty(AutoNumberTB.Text) ||
                string.IsNullOrEmpty(AutoYearTB.Text) ||
                string.IsNullOrEmpty(AutoCostTB.Text))
            {
                return false;
            }

            if (!int.TryParse(IdPoliciesNewTB.Text, out _) ||
                !decimal.TryParse(SummaPolNewTB.Text, out _) ||
                !int.TryParse(IdAgentNewTB.Text, out _) ||
                !int.TryParse(IdClientNewTB.Text, out _) ||
                !int.TryParse(AutoYearTB.Text, out _) ||
                !decimal.TryParse(AutoCostTB.Text, out _))
            {
                return false;
            }

            return true;
        }
        private void ChangeAutoBTN_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAutoInput())
            {
                int idPolicies;
                if (!int.TryParse(IdPoliciesNewTB.Text, out idPolicies))
                {
                    MessageBox.Show("Введіть коректний Id поліса для зміни.");
                    return;
                }
                string autoNumber = AutoNumberTB.Text;
                int autoYear;
                if (!int.TryParse(AutoYearTB.Text, out autoYear) || autoYear < 1970 || autoYear > DateTime.Now.Year)
                {
                    MessageBox.Show("Рік виробництва автомобіля має бути після 1970 і не більше поточного року.");
                    return;
                }
                decimal autoCost;
                if (!decimal.TryParse(AutoCostTB.Text, out autoCost))
                {
                    MessageBox.Show("Введіть коректну вартість автомобіля.");
                    return;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE InsuranceAuto SET AutoNumber = @AutoNumber, AutoYear = @AutoYear, AutoCost = @AutoCost WHERE IdPolicies = @IdPolicies";

                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@AutoNumber", autoNumber);
                        cmd.Parameters.AddWithValue("@AutoYear", autoYear);
                        cmd.Parameters.AddWithValue("@AutoCost", autoCost);
                        cmd.Parameters.AddWithValue("@IdPolicies", idPolicies);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Дані автомобіля успішно змінені!");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Неможливо змінити дані. Перевірте ID");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при зміні даних автомобіля: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Перевірте правильність введення даних.");
            }
        }
        private bool ValidateAutoInput()
        {
            if (string.IsNullOrEmpty(IdPoliciesNewTB.Text) ||
                string.IsNullOrEmpty(AutoNumberTB.Text) ||
                string.IsNullOrEmpty(AutoYearTB.Text) ||
                string.IsNullOrEmpty(AutoCostTB.Text))
            {
                return false;
            }

            if (!int.TryParse(IdPoliciesNewTB.Text, out _) ||
                !int.TryParse(AutoYearTB.Text, out _) ||
                !decimal.TryParse(AutoCostTB.Text, out _))
            {
                return false;
            }

            return true;
        }
        private void AutoTBL1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AutoTBL1.SelectedItem != null && AutoTBL1.SelectedItem is DataRowView)
            {
                DataRowView row = (DataRowView)AutoTBL1.SelectedItem;

                MessageBoxResult result = MessageBox.Show("Хочете змінити дані цього автомобіля?", "Зміна данних", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    IdPoliciesNewTB.Text = row["IdPolicies"].ToString();
                    AutoNumberTB.Text = row["AutoNumber"].ToString();
                    AutoYearTB.Text = row["AutoYear"].ToString();
                    AutoCostTB.Text = row["AutoCost"].ToString();
                }
            }
            else
            {
                MessageBox.Show("Тут ще немає автомобіля.");
            }
        }

        // Взаємодія з табл InsuranceLife
        private void AddPoliciesHumanBTN_Click(object sender, RoutedEventArgs e)
        {
            int idPolicies;
            decimal summaPol;
            int idAgent;
            int idClient;
            DateTime startDate;
            DateTime endDate;
            string lifeFiName;
            string lifeSeName;
            string lifeFaName;
            DateTime lifeBithDay;

            if (!int.TryParse(IdPoliciesNewTB.Text, out idPolicies) ||
                !decimal.TryParse(SummaPolNewTB.Text, out summaPol) ||
                !int.TryParse(IdAgentNewTB.Text, out idAgent) ||
                !int.TryParse(IdClientNewTB.Text, out idClient) ||
                !DateTime.TryParse(DateStartDP.Text, out startDate) ||
                !DateTime.TryParse(DateEndDP.Text, out endDate) ||
                !DateTime.TryParse(LifeBithDayDP.Text, out lifeBithDay) ||
                string.IsNullOrEmpty(lifeFiName = LifeFiNameTB.Text.Trim()) ||
                string.IsNullOrEmpty(lifeSeName = LifeSeNameTB.Text.Trim()) ||
                string.IsNullOrEmpty(lifeFaName = LifeFaNameTB.Text.Trim()) ||
                char.IsLower(lifeFiName[0]) || char.IsLower(lifeSeName[0]) || char.IsLower(lifeFaName[0]) ||
                lifeBithDay.Year < 1930 || lifeBithDay > DateTime.Now || summaPol <= 0)
            {
                MessageBox.Show("Перевірте правильність введення даних про поліс та людину.");
                return;
            }

            if (startDate > endDate || endDate < DateTime.Now || startDate < DateTime.Now)
            {
                MessageBox.Show("Дата початку страховки має бути меншою за дату кінця та не може бути минулою.");
                return;
            }

            string status;
            DateTime currentDate = DateTime.Now;

            if (currentDate < startDate)
            {
                status = "Неактивний";
            }
            else if (currentDate >= startDate && currentDate <= endDate)
            {
                status = "Активний";
            }
            else
            {
                status = "Прострочений";
            }
            string connectionString = "YourConnectionString";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    string checkExistingPolicyQuery = "SELECT COUNT(*) FROM Policies WHERE IdPolicies = @IdPolicies";
                    SqlCommand checkExistingPolicyCommand = new SqlCommand(checkExistingPolicyQuery, connection, transaction);
                    checkExistingPolicyCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                    int existingPolicyCount = (int)checkExistingPolicyCommand.ExecuteScalar();
                    if (existingPolicyCount > 0)
                    {
                        MessageBox.Show("Поліс з таким ID вже існує.");
                        return;
                    }
                    string insertPoliciesQuery = "INSERT INTO Policies (IdPolicies, SummaPol, IdAgent, IdClients, StartDate, EndDate, PoliciesType, PolicyStatus) " +
                                                 "VALUES (@IdPolicies, @SummaPol, @IdAgent, @IdClients, @StartDate, @EndDate, @PoliciesType, @PolicyStatus)";
                    SqlCommand insertPoliciesCommand = new SqlCommand(insertPoliciesQuery, connection, transaction);
                    insertPoliciesCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                    insertPoliciesCommand.Parameters.AddWithValue("@SummaPol", summaPol);
                    insertPoliciesCommand.Parameters.AddWithValue("@IdAgent", idAgent);
                    insertPoliciesCommand.Parameters.AddWithValue("@IdClients", idClient);
                    insertPoliciesCommand.Parameters.AddWithValue("@PoliciesType", "Страхування життя");
                    insertPoliciesCommand.Parameters.AddWithValue("@StartDate", startDate);
                    insertPoliciesCommand.Parameters.AddWithValue("@EndDate", endDate);
                    insertPoliciesCommand.Parameters.AddWithValue("@PolicyStatus", status);
                    insertPoliciesCommand.ExecuteNonQuery();

                    string insertInsuranceLifeQuery = "INSERT INTO InsuranceLife (IdPolicies, LifeFiName, LifeSeName, LifeFaName, LifeBirthDay) " +
                                                      "VALUES (@IdPolicies, @LifeFiName, @LifeSeName, @LifeFaName, @LifeBirthDay)";
                    SqlCommand insertInsuranceLifeCommand = new SqlCommand(insertInsuranceLifeQuery, connection, transaction);
                    insertInsuranceLifeCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                    insertInsuranceLifeCommand.Parameters.AddWithValue("@LifeFiName", lifeFiName);
                    insertInsuranceLifeCommand.Parameters.AddWithValue("@LifeSeName", lifeSeName);
                    insertInsuranceLifeCommand.Parameters.AddWithValue("@LifeFaName", lifeFaName);
                    insertInsuranceLifeCommand.Parameters.AddWithValue("@LifeBirthDay", lifeBithDay);
                    insertInsuranceLifeCommand.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Дані успішно додані.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні даних про поліс: " + ex.Message);
                    transaction?.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void ChangeHumanBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdPoliciesNewTB.Text))
            {
                int idPolicies = int.Parse(IdPoliciesNewTB.Text);
                string lifeFiName = LifeFiNameTB.Text;
                string lifeSeName = LifeSeNameTB.Text;
                string lifeFaName = LifeFaNameTB.Text;
                DateTime LifeBirthDay = LifeBithDayDP.SelectedDate ?? DateTime.Now;

                if (!ValidateHumanInput(lifeFiName, lifeSeName, lifeFaName, LifeBirthDay))
                {
                    MessageBox.Show("Перевірте правильність введення даних про людину.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE InsuranceLife SET LifeFiName = @LifeFiName, LifeSeName = @LifeSeName, LifeFaName = @LifeFaName, LifeBirthDay = @LifeBirthDay WHERE IdPolicies = @IdPolicies";

                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@LifeFiName", lifeFiName);
                        cmd.Parameters.AddWithValue("@LifeSeName", lifeSeName);
                        cmd.Parameters.AddWithValue("@LifeFaName", lifeFaName);
                        cmd.Parameters.AddWithValue("@LifeBirthDay", LifeBirthDay);
                        cmd.Parameters.AddWithValue("@IdPolicies", idPolicies);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Дані про людину та поліс успішно змінені!");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Неможливо змінити дані. Перевірте ID");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при зміні даних про людину: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Введіть Id поліса для зміни.");
            }
        }
        private bool ValidateHumanInput(string firstName, string secondName, string familyName, DateTime birthday)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(secondName) || string.IsNullOrEmpty(familyName))
            {
                return false;
            }

            if (char.IsLower(firstName[0]) || char.IsLower(secondName[0]) || char.IsLower(familyName[0]))
            {
                return false;
            }

            if (birthday < new DateTime(1930, 1, 1) || birthday > DateTime.Now)
            {
                return false;
            }

            return true;
        }
        private void LifeTBL1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LifeTBL1.SelectedItem != null && LifeTBL1.SelectedItem is DataRowView)
            {
                DataRowView row = (DataRowView)LifeTBL1.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Хочете змінити дані цього клієнта?", "Зміна даних", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    IdPoliciesNewTB.Text = row["IdPolicies"].ToString();
                    LifeFiNameTB.Text = row["LifeFiName"].ToString();
                    LifeSeNameTB.Text = row["LifeSeName"].ToString();
                    LifeFaNameTB.Text = row["LifeFaName"].ToString();
                    LifeBithDayDP.SelectedDate = row["LifeBirthDay"] != DBNull.Value ? (DateTime)row["LifeBirthDay"] : DateTime.Now;
                }
            }
            else
            {
                MessageBox.Show("Тут ще немає клієнта.");
            }
        }

        // Взаємодія з табл InsuranceHome
        private void AddPoliciesHomeBTN_Click(object sender, RoutedEventArgs e)
        {
            int idPolicies;
            string homeAddress;
            decimal homeCost;
            int summaPol;
            int idAgent;
            int idClient;
            DateTime dateStart;
            DateTime dateEnd;

            if (!int.TryParse(IdPoliciesNewTB.Text, out idPolicies) ||
                !int.TryParse(SummaPolNewTB.Text, out summaPol) ||
                !int.TryParse(IdAgentNewTB.Text, out idAgent) ||
                !int.TryParse(IdClientNewTB.Text, out idClient) ||
                !DateTime.TryParse(DateStartDP.Text, out dateStart) ||
                !DateTime.TryParse(DateEndDP.Text, out dateEnd) ||
                string.IsNullOrEmpty(homeAddress = HomeAddressrTB.Text.Trim()) ||
                !decimal.TryParse(HomeCostTB.Text, out homeCost) ||
                !Regex.IsMatch(homeAddress, @"вул\.\s\w+,\s\d+,\s\d+") || summaPol <= 0)
            {
                MessageBox.Show("Перевірте правильність введення даних про страхування житла.");
                return;
            }

            if (dateStart > dateEnd || dateEnd < DateTime.Now || dateStart < DateTime.Now)
            {
                MessageBox.Show("Дата початку страховки має бути меншою за дату кінця та не може бути минулою.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlTransaction transaction = null;

                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();
                    string checkIdQuery = "SELECT COUNT(*) FROM Policies WHERE IdPolicies = @IdPolicies";
                    SqlCommand checkIdCommand = new SqlCommand(checkIdQuery, connection, transaction);
                    checkIdCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);

                    int existingRecords = (int)checkIdCommand.ExecuteScalar();
                    if (existingRecords > 0)
                    {
                        MessageBox.Show("Посилка з вказаним ID вже існує.");
                        return;
                    }
                    string status;
                    DateTime currentDate = DateTime.Now;

                    if (currentDate < dateStart)
                    {
                        status = "Неактивний";
                    }
                    else if (currentDate >= dateStart && currentDate <= dateEnd)
                    {
                        status = "Активний";
                    }
                    else
                    {
                        status = "Прострочений";
                    }

                    string insertPoliciesQuery = "INSERT INTO Policies (IdPolicies, SummaPol, IdAgent, IdClients, StartDate, EndDate, PoliciesType, PolicyStatus) " +
                                                 "VALUES (@IdPolicies, @SummaPol, @IdAgent, @IdClients, @StartDate, @EndDate, @PoliciesType, @PolicyStatus)";
                    SqlCommand insertPoliciesCommand = new SqlCommand(insertPoliciesQuery, connection, transaction);
                    insertPoliciesCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                    insertPoliciesCommand.Parameters.AddWithValue("@SummaPol", summaPol);
                    insertPoliciesCommand.Parameters.AddWithValue("@IdAgent", idAgent);
                    insertPoliciesCommand.Parameters.AddWithValue("@IdClients", idClient);
                    insertPoliciesCommand.Parameters.AddWithValue("@PoliciesType", "Страхування житла");
                    insertPoliciesCommand.Parameters.AddWithValue("@StartDate", dateStart);
                    insertPoliciesCommand.Parameters.AddWithValue("@EndDate", dateEnd);
                    insertPoliciesCommand.Parameters.AddWithValue("@PolicyStatus", status);
                    insertPoliciesCommand.ExecuteNonQuery();

                    string insertInsuranceHomeQuery = "INSERT INTO InsuranceHome (IdPolicies, HomeAddress, HomeCost) VALUES (@IdPolicies, @HomeAddress, @HomeCost)";
                    SqlCommand insertInsuranceHomeCommand = new SqlCommand(insertInsuranceHomeQuery, connection, transaction);
                    insertInsuranceHomeCommand.Parameters.AddWithValue("@IdPolicies", idPolicies);
                    insertInsuranceHomeCommand.Parameters.AddWithValue("@HomeAddress", homeAddress);
                    insertInsuranceHomeCommand.Parameters.AddWithValue("@HomeCost", homeCost);
                    insertInsuranceHomeCommand.ExecuteNonQuery();

                    transaction.Commit();
                    MessageBox.Show("Дані про страхування житла успішно додані.");
                    LoadData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при додаванні даних про страхування житла: " + ex.Message);
                    transaction?.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        private void ChangeHomeBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdPoliciesNewTB.Text))
            {
                int idPolicies;
                string homeAddress;
                decimal homeCost;

                if (!int.TryParse(IdPoliciesNewTB.Text, out idPolicies) ||
                    string.IsNullOrEmpty(homeAddress = HomeAddressrTB.Text.Trim()) ||
                    !decimal.TryParse(HomeCostTB.Text, out homeCost) ||
                    !Regex.IsMatch(homeAddress, @"^вул\.\s[A-Za-zА-Яа-я]+\,\s\d+\,\s\d+$"))
                {
                    MessageBox.Show("Перевірте правильність введення даних про страхування житла.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE InsuranceHome SET HomeAddress = @HomeAddress, HomeCost = @HomeCost WHERE IdPolicies = @IdPolicies";

                        SqlCommand cmd = new SqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@HomeAddress", homeAddress);
                        cmd.Parameters.AddWithValue("@HomeCost", homeCost);
                        cmd.Parameters.AddWithValue("@IdPolicies", idPolicies);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Дані про страхування житла успішно змінені!");
                            LoadData();
                        }
                        else
                        {
                            MessageBox.Show("Неможливо змінити дані. Перевірте ID");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при зміні даних про страхування житла: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Введіть Id поліса для зміни.");
            }
        }
        private void HomeTBL1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (HomeTBL1.SelectedItem != null && HomeTBL1.SelectedItem is DataRowView)
            {
                DataRowView row = (DataRowView)HomeTBL1.SelectedItem;
                MessageBoxResult result = MessageBox.Show("Хочете змінити дані цього об'єкта?", "Зміна даних", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    IdPoliciesNewTB.Text = row["IdPolicies"].ToString();
                    HomeAddressrTB.Text = row["HomeAddress"].ToString();
                    HomeCostTB.Text = row["HomeCost"].ToString();
                }
            }
            else
            {
                MessageBox.Show("Тут ще немає квартири.");
            }
        }



        // Взаємодія з табл InsuranceEvent
        private void AddEventBTN_Click(object sender, RoutedEventArgs e)
        {
            int idPoliciesEvent;
            int idEvent;
            decimal eventSumma;
            string eventReason;
            DateTime eventDate;

            if (!int.TryParse(IdPoliciesEventTB.Text, out idPoliciesEvent) ||
                !int.TryParse(IdEventTB.Text, out idEvent) ||
                !decimal.TryParse(EventSummaTB.Text, out eventSumma) ||
                string.IsNullOrEmpty(eventReason = EventReasonTB.Text.Trim()) ||
                !DateTime.TryParse(EventDP.Text, out eventDate) ||
                eventSumma <= 0)
            {
                MessageBox.Show("Перевірте правильність введення даних про страховий випадок.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string checkPolicyQuery = "SELECT COUNT(*) FROM Policies WHERE IdPolicies = @IdPolicies";
                    SqlCommand checkPolicyCommand = new SqlCommand(checkPolicyQuery, connection);
                    checkPolicyCommand.Parameters.AddWithValue("@IdPolicies", idPoliciesEvent);
                    int policyCount = (int)checkPolicyCommand.ExecuteScalar();

                    if (policyCount == 0)
                    {
                        MessageBox.Show("Поліс з вказаним ID не існує.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка перевірки наявності поліса: " + ex.Message);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkEventQuery = "SELECT COUNT(*) FROM InsuranceEvent WHERE IdPolicies = @IdPolicies AND IdEvent = @IdEvent";
                    SqlCommand checkEventCommand = new SqlCommand(checkEventQuery, connection);
                    checkEventCommand.Parameters.AddWithValue("@IdPolicies", idPoliciesEvent);
                    checkEventCommand.Parameters.AddWithValue("@IdEvent", idEvent);

                    int eventCount = (int)checkEventCommand.ExecuteScalar();

                    if (eventCount > 0)
                    {
                        MessageBox.Show("Страховий випадок з вказаним ID вже існує.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка перевірки наявності страхового випадку: " + ex.Message);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string insertEventQuery = "INSERT INTO InsuranceEvent (IdPolicies, IdEvent, EventDate, EventSumma, EventReason) " +
                                              "VALUES (@IdPolicies, @IdEvent, @EventDate, @EventSumma, @EventReason)";
                    SqlCommand insertEventCommand = new SqlCommand(insertEventQuery, connection);
                    insertEventCommand.Parameters.AddWithValue("@IdPolicies", idPoliciesEvent);
                    insertEventCommand.Parameters.AddWithValue("@IdEvent", idEvent);
                    insertEventCommand.Parameters.AddWithValue("@EventDate", eventDate);
                    insertEventCommand.Parameters.AddWithValue("@EventSumma", eventSumma);
                    insertEventCommand.Parameters.AddWithValue("@EventReason", eventReason);

                    int rowsAffected = insertEventCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Страховий випадок успішно доданий.");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося додати страховий випадок.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні страхового випадку: " + ex.Message);
            }
        }
        private void ChangeEventBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdPoliciesEventTB.Text) && !string.IsNullOrEmpty(IdEventTB.Text)
                && !string.IsNullOrEmpty(EventSummaTB.Text) && !string.IsNullOrEmpty(EventReasonTB.Text)
                && EventDP.SelectedDate != null)
            {
                int idPolicies, idEvent;
                decimal eventSumma;
                DateTime eventDate;
                if (!int.TryParse(IdPoliciesEventTB.Text, out idPolicies) || !int.TryParse(IdEventTB.Text, out idEvent)
                    || !decimal.TryParse(EventSummaTB.Text, out eventSumma) || !DateTime.TryParse(EventDP.Text, out eventDate))
                {
                    MessageBox.Show("Невірний формат введених даних.");
                    return;
                }
                if (eventSumma <= 0)
                {
                    MessageBox.Show("Сума випадка повина бути додатковим числом.");
                    return;
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string checkPolicyQuery = "SELECT COUNT(*) FROM Policies WHERE IdPolicies = @IdPolicies";
                        SqlCommand checkPolicyCmd = new SqlCommand(checkPolicyQuery, connection);
                        checkPolicyCmd.Parameters.AddWithValue("@IdPolicies", idPolicies);

                        int policyCount = (int)checkPolicyCmd.ExecuteScalar();

                        if (policyCount == 0)
                        {
                            MessageBox.Show("Поліс з таким ID не існує.");
                            return;
                        }
                        string checkEventQuery = "SELECT COUNT(*) FROM InsuranceEvent WHERE IdPolicies = @IdPolicies AND IdEvent = @IdEvent";
                        SqlCommand checkEventCmd = new SqlCommand(checkEventQuery, connection);
                        checkEventCmd.Parameters.AddWithValue("@IdPolicies", idPolicies);
                        checkEventCmd.Parameters.AddWithValue("@IdEvent", idEvent);

                        int eventCount = (int)checkEventCmd.ExecuteScalar();
                        if (eventCount > 0)
                        {
                            string updateEventQuery = "UPDATE InsuranceEvent SET EventDate = @EventDate, EventSumma = @EventSumma, EventReason = @EventReason WHERE IdPolicies = @IdPolicies AND IdEvent = @IdEvent";
                            SqlCommand updateEventCmd = new SqlCommand(updateEventQuery, connection);
                            updateEventCmd.Parameters.AddWithValue("@IdPolicies", idPolicies);
                            updateEventCmd.Parameters.AddWithValue("@IdEvent", idEvent);
                            updateEventCmd.Parameters.AddWithValue("@EventDate", eventDate);
                            updateEventCmd.Parameters.AddWithValue("@EventSumma", eventSumma);
                            updateEventCmd.Parameters.AddWithValue("@EventReason", EventReasonTB.Text);

                            int rowsAffected = updateEventCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Зміни страхового випадку успішно внесено.");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Не вдалося змінити страховий випадок.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Страховий випадок з таким ID не існує.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при зміні страхового випадку: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Заповніть усі поля для зміни страхового випадку.");
            }
        }
        private void DeleteEventBTN_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(IdEventTB.Text) && int.TryParse(IdEventTB.Text, out int idEvent))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        SqlCommand checkEventCommand = new SqlCommand("SELECT IdEvent FROM InsuranceEvent WHERE IdEvent = @IdEvent;", connection);
                        checkEventCommand.Parameters.AddWithValue("@IdEvent", idEvent);
                        object eventResult = checkEventCommand.ExecuteScalar();

                        if (eventResult != null)
                        {
                            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете видалити цей страховий випадок?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                SqlCommand deleteEventCommand = new SqlCommand("DELETE FROM InsuranceEvent WHERE IdEvent = @IdEvent;", connection);
                                deleteEventCommand.Parameters.AddWithValue("@IdEvent", idEvent);
                                deleteEventCommand.ExecuteNonQuery();

                                MessageBox.Show("Страховий випадок успішно видалено.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Страховий випадок з таким ID не існує.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка видалення страхового випадку: " + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Введіть Id страхового випадку для видалення.");
            }
        }
        private void InsuranceEventTBL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InsuranceEventTBL.SelectedItem != null && InsuranceEventTBL.SelectedItem is DataRowView)
            {
                DataRowView row = (DataRowView)InsuranceEventTBL.SelectedItem;
                if (row["IdPolicies"] != DBNull.Value && row["IdEvent"] != DBNull.Value &&
                    row["EventSumma"] != DBNull.Value && row["EventDate"] != DBNull.Value &&
                    row["EventReason"] != DBNull.Value)
                {
                    MessageBoxResult result = MessageBox.Show("Хочете змінити дані цього страхового випадку?", "Зміна даних", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        IdPoliciesEventTB.Text = row["IdPolicies"].ToString();
                        IdEventTB.Text = row["IdEvent"].ToString();
                        EventSummaTB.Text = row["EventSumma"].ToString();
                        EventReasonTB.Text = row["EventReason"].ToString();
                        EventDP.SelectedDate = (DateTime)row["EventDate"];
                    }
                }
                else
                {
                    MessageBox.Show("Тут ще немає даних про страховий випадок.");
                }
            }
        }
    }
}