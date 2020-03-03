using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Collections;

// Manages ADO.NET SQL Server Database operations.
public class SQLdbService
{
    #region Fields
    const int successful = 1; // For successful SQL Server Database operations.
    const int unsuccessful = -2; // For unsuccessful SQL Server Database operations.
    const int exception = -1; // For exceptions that occur during SQL Server Database operations.
    const int mistake = -3; // For coding mistakes that may effect the operation during run-time.
    SqlConnection connection;
    SqlCommand command;
    SqlDataAdapter dataAdapter;
    SqlParameter[] parameters;
    SqlTransaction transaction;
    int parameterIndex;
    bool createParametersCalled;
    SqlCommand[] commands;
    int commandIndex;
    bool createCommandsCalled;
    bool keepLog; // Flag that indicates whether log in a file will be kept or not when a SQL Server Database operation throws exception.
    bool connectionOpenedOutside; // Flag that indicates whether the connection is opened from outside the SQLdatabase class or not.
    #endregion

    #region Constructors
    // Gets the first connection string and creates a new SQL Database connection.
    public SQLdbService(int timeout = 180, bool writeToLog = false)
    {
        connection = null;
        command = null;
        dataAdapter = null;
        parameters = null;
        transaction = null;
        commands = null;
        string temporaryConnectionString = "";
        keepLog = writeToLog;
        if (ConfigurationManager.ConnectionStrings.Count > 0)
        {
            temporaryConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            connection = new SqlConnection(temporaryConnectionString);
            createParametersCalled = false;
            command = new SqlCommand();
            command.Connection = connection;
            command.CommandTimeout = timeout;
            dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = command;
            createCommandsCalled = false;
            connectionOpenedOutside = false;
        }
        else
        {
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForGeneral("NO CONNECTION STRING");
        }
    }

    // Creates a new SQL Database connection according to the connection string parameter.
    public SQLdbService(string connectionString, int timeout = 180, bool writeToLog = false)
    {
        connection = null;
        command = null;
        dataAdapter = null;
        parameters = null;
        transaction = null;
        commands = null;
        string temporaryConnectionString = "";
        bool found = false;
        keepLog = writeToLog;
        if (ConfigurationManager.ConnectionStrings.Count > 0)
        {
            if (connectionString.Contains(";"))
            {
                temporaryConnectionString = connectionString;
                found = true;
            }
            else
            {
                for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count && !found; i++)
                {
                    if (ConfigurationManager.ConnectionStrings[i].Name.Equals(connectionString))
                    {
                        temporaryConnectionString = ConfigurationManager.ConnectionStrings[i].ConnectionString;
                        found = true;
                    }
                }
            }
            if (found)
            {
                connection = new SqlConnection(temporaryConnectionString);
                createParametersCalled = false;
                command = new SqlCommand();
                command.Connection = connection;
                command.CommandTimeout = timeout;
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = command;
                createCommandsCalled = false;
                connectionOpenedOutside = false;
            }
            else
            {
                int writeToLogFileResult;
                if (keepLog == true)
                    writeToLogFileResult = writeToLogFileForGeneral("WRONG CONNECTION STRING OR DATABASE NAME");
            }
        }
        else
        {
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForGeneral("NO CONNECTION STRING");
        }
    }
    #endregion

    #region Methods
    // Destruction of the SQLdatabase object.
    public void Destroy()
    {
        parameters = null;
        commands = null;
        if (connection != null)
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
            connection.Dispose();
            connection = null;
        }
        if (transaction != null)
        {
            transaction.Dispose();
            transaction = null;
        }
        if (dataAdapter != null)
        {
            dataAdapter.Dispose();
            dataAdapter = null;
        }
        if (command != null)
        {
            command.Dispose();
            command = null;
        }
    }

    // Manages logging to the log file for SQL queries.
    private int writeToLogFileForQuery(string query, string exceptionMessage)
    {
        int result = mistake;
        if (!query.Equals("") && !exceptionMessage.Equals(""))
        {
            result = unsuccessful;
            //string logFilePath = HttpRuntime.AppDomainAppPath + @"\SQLdatabaseLOG.txt";
            string logFilePath = ConfigurationManager.AppSettings["SQLDBLOGFILEPATH"];
            string logMessage = "";
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string parameterList = "";
                if (parameters != null)
                {
                    if (parameters.Length > 0)
                    {
                        parameterList += "~";
                        foreach (SqlParameter parameter in parameters)
                        {
                            parameterList += parameter.Value.ToString();
                            parameterList += "~";
                        }
                    }
                }
                if (parameterList.Equals(""))
                    parameterList = "NO PARAMETERS";
                logMessage = string.Format("{0} | {1} | {2} | {3}", DateTime.Now, query, parameterList, exceptionMessage);
                if (File.Exists(logFilePath))
                    fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
                else
                    fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(logMessage);
                streamWriter.WriteLine();
                result = successful;
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                result = exception;
            }
            finally
            {
                if (streamWriter != null)
                    streamWriter.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        return result;
    }

    // Manages logging to the log file for SQL Server Database connections.
    private int writeToLogFileForConnection(string connectionString, string exceptionMessage)
    {
        int result = mistake;
        if (!connectionString.Equals("") && !exceptionMessage.Equals(""))
        {
            result = unsuccessful;
            //string logFilePath = HttpRuntime.AppDomainAppPath + @"\SQLdatabaseLOG.txt";
            string logFilePath = ConfigurationManager.AppSettings["SQLDBLOGFILEPATH"];
            string logMessage = "";
            logMessage = string.Format("{0} | {1} | {2}", DateTime.Now, connectionString, exceptionMessage);
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                if (File.Exists(logFilePath))
                    fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
                else
                    fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(logMessage);
                streamWriter.WriteLine();
                result = successful;
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                result = exception;
            }
            finally
            {
                if (streamWriter != null)
                    streamWriter.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        return result;
    }

    // Manages logging to the log file for general operations.
    private int writeToLogFileForGeneral(string generalMessage)
    {
        int result = mistake;
        if (!generalMessage.Equals(""))
        {
            result = unsuccessful;
            //string logFilePath = HttpRuntime.AppDomainAppPath + @"\SQLdatabaseLOG.txt";
            string logFilePath = ConfigurationManager.AppSettings["SQLDBLOGFILEPATH"];
            string logMessage = "";
            logMessage = string.Format("{0} | {1}", DateTime.Now, generalMessage);
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                if (File.Exists(logFilePath))
                    fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write);
                else
                    fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write);
                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(logMessage);
                streamWriter.WriteLine();
                result = successful;
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                result = exception;
            }
            finally
            {
                if (streamWriter != null)
                    streamWriter.Close();
                if (fileStream != null)
                    fileStream.Close();
            }
        }
        return result;
    }

    // Creates the empty parameter list required by the SQL or Stored Procedure.
    public int createParameters(int parameterCount)
    {
        int result = mistake;
        if (parameterCount > 0)
        {
            result = unsuccessful;
            try
            {
                parameters = new SqlParameter[parameterCount];
                parameterIndex = 0;
                createParametersCalled = true;
                result = successful;
            }
            catch (Exception exc)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connectionOpenedOutside = false;
                    }
                }
                string message = exc.Message;
                result = exception;
            }
        }
        return result;
    }

    // Adds the parameter to the parameter list required by the SQL or Stored Procedure.
    // Works for only SQL data types like varchar and nvarchar.
    // createParameters method must be called before.
    public int addParameter(string parameterName, object parameterValue)
    {
        int result = mistake;
        if (createParametersCalled)
        {
            result = unsuccessful;
            try
            {
                SqlParameter parameter = new SqlParameter(parameterName, parameterValue);
                parameters[parameterIndex++] = parameter;
                result = successful;
            }
            catch (Exception exc)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connectionOpenedOutside = false;
                    }
                }
                string message = exc.Message;
                result = exception;
            }
        }
        return result;
    }

    // Adds the parameter to the parameter list required by the SQL or Stored Procedure.
    // SQL data type is passed as a method parameter.
    // createParameters method must be called before.
    public int addParameter(string parameterName, object parameterValue, SqlDbType parameterType)
    {
        int result = mistake;
        if (createParametersCalled)
        {
            result = unsuccessful;
            try
            {
                SqlParameter parameter = new SqlParameter(parameterName, parameterValue);
                parameter.SqlDbType = parameterType;
                parameters[parameterIndex++] = parameter;
                result = successful;
            }
            catch (Exception exc)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connectionOpenedOutside = false;
                    }
                }
                string message = exc.Message;
                result = exception;
            }
        }
        return result;
    }

    // For opening the connection from outside the SQLdatabase class.
    public int openConnection()
    {
        int result = mistake;
        if (connection != null)
        {
            result = unsuccessful;
            try
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connection.Open();
                connectionOpenedOutside = true;
                result = successful;
            }
            catch (Exception exc)
            {
                string connectionString;
                if (connection != null)
                {
                    if (!connection.ConnectionString.Trim().Equals(""))
                        connectionString = connection.ConnectionString;
                    else
                        connectionString = "NO CONNECTION STRING";
                }
                else
                {
                    connectionString = "NO CONNECTION STRING";
                }
                string message = exc.Message;
                int writeToLogFileResult;
                if (keepLog == true)
                    writeToLogFileResult = writeToLogFileForConnection(connectionString, message);
                result = exception;
            }
        }
        return result;
    }

    // For closing the connection from outside the SQLdatabase class.
    public int closeConnection()
    {
        int result = mistake;
        if (connection != null)
        {
            result = unsuccessful;
            try
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
                connectionOpenedOutside = false;
                result = successful;
            }
            catch (Exception exc)
            {
                string connectionString;
                if (connection != null)
                {
                    if (!connection.ConnectionString.Trim().Equals(""))
                        connectionString = connection.ConnectionString;
                    else
                        connectionString = "NO CONNECTION STRING";
                }
                else
                {
                    connectionString = "NO CONNECTION STRING";
                }
                string message = exc.Message;
                int writeToLogFileResult;
                if (keepLog == true)
                    writeToLogFileResult = writeToLogFileForConnection(connectionString, message);
                result = exception;
            }
        }
        return result;
    }

    // For insert, update and delete operations.
    // SQL: Not Stored Procedure.
    public int SQL_InsertUpdateDelete(string sql)
    {
        int result = mistake;
        if (sql.ToLower(new System.Globalization.CultureInfo("en-US", false)).Contains("insert") || sql.ToLower().Contains("update") || sql.ToLower().Contains("delete"))
        {
            result = unsuccessful;
            int effectedRowCount = -99;
            try
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.Parameters.Clear();
                if (parameters != null)
                {
                    if (parameters.Length > 0)
                    {
                        foreach (SqlParameter parameter in parameters)
                            command.Parameters.Add(parameter);
                    }
                }
                if (connectionOpenedOutside == false)
                    connection.Open();
                effectedRowCount = command.ExecuteNonQuery();
                result = effectedRowCount;
                createParametersCalled = false;
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                int writeToLogFileResult;
                if (keepLog == true)
                    writeToLogFileResult = writeToLogFileForQuery(sql, message);
                result = exception;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        if (connectionOpenedOutside == false || result == exception)
                        {
                            connection.Close();
                            if (result == exception)
                                connectionOpenedOutside = false;
                        }
                    }
                }
            }
        }
        return result;
    }

    // For insert, update and delete operations.
    // Returns no stored procedure result value. 
    // PROC: Stored Procedure.
    public int PROC_InsertUpdateDelete(string procedureName)
    {
        int result = unsuccessful;
        int effectedRowCount = -99;
        try
        {
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
            }
            if (connectionOpenedOutside == false)
                connection.Open();
            effectedRowCount = command.ExecuteNonQuery();
            result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        finally
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    if (connectionOpenedOutside == false || result == exception)
                    {
                        connection.Close();
                        if (result == exception)
                            connectionOpenedOutside = false;
                    }
                }
            }
        }
        return result;
    }

    // For insert, update and delete operations.
    // Returns stored procedure result value or method result value according to the isResultStoredProcedureResult parameter.
    // PROC: Stored Procedure.
    public int PROC_InsertUpdateDelete(string procedureName, bool isResultProcedureResult)
    {
        int result = unsuccessful;
        int effectedRowCount = -99;
        try
        {
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
            }
            if (isResultProcedureResult)
            {
                SqlParameter procedureResultParameter = new SqlParameter("@procedureResult", SqlDbType.Int);
                procedureResultParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(procedureResultParameter);
            }
            if (connectionOpenedOutside == false)
                connection.Open();
            effectedRowCount = command.ExecuteNonQuery();
            if (isResultProcedureResult)
                result = (int)command.Parameters["@procedureResult"].Value;
            else
                result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        finally
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    if (connectionOpenedOutside == false || result == exception)
                    {
                        connection.Close();
                        if (result == exception)
                            connectionOpenedOutside = false;
                    }
                }
            }
        }
        return result;
    }

    // For select operation that returns a DataSet.
    // SQL: Not Stored Procedure.
    public DataSet SQL_SelectAdapter(string sql, out int result)
    {
        result = mistake;
        DataSet dataSet;
        dataSet = new DataSet();
        if (sql.ToLower().Contains("select"))
        {
            result = unsuccessful;
            SqlConnection temporaryConnection = null;
            try
            {
                if (connectionOpenedOutside == true)
                {
                    temporaryConnection = connection;
                    dataAdapter.SelectCommand.Connection = temporaryConnection;
                }
                dataAdapter.SelectCommand.CommandText = sql;
                dataAdapter.SelectCommand.CommandType = CommandType.Text;
                dataAdapter.SelectCommand.Parameters.Clear();
                if (parameters != null)
                {
                    if (parameters.Length > 0)
                    {
                        foreach (SqlParameter parameter in parameters)
                            dataAdapter.SelectCommand.Parameters.Add(parameter);
                    }
                }
                dataAdapter.Fill(dataSet);
                result = successful;
                createParametersCalled = false;
            }
            catch (Exception exc)
            {
                if (connectionOpenedOutside == true)
                {
                    if (connection != null)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                            connectionOpenedOutside = false;
                        }
                    }
                }
                dataSet = null;
                string message = exc.Message;
                int writeToLogFileResult;
                if (keepLog == true)
                    writeToLogFileResult = writeToLogFileForQuery(sql, message);
                result = exception;
            }
        }
        return dataSet;
    }

    // For select operation that returns a DataSet.
    // Returns no stored procedure result value as output.
    // PROC: Stored Procedure.
    public DataSet PROC_SelectAdapter(string procedureName, out int result)
    {
        result = unsuccessful;
        DataSet dataSet;
        SqlConnection temporaryConnection = null;
        try
        {
            if (connectionOpenedOutside == true)
            {
                temporaryConnection = connection;
                dataAdapter.SelectCommand.Connection = temporaryConnection;
            }
            dataSet = new DataSet();
            dataAdapter.SelectCommand.CommandText = procedureName;
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        dataAdapter.SelectCommand.Parameters.Add(parameter);
                }
            }
            dataAdapter.Fill(dataSet);
            result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            if (connectionOpenedOutside == true)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connectionOpenedOutside = false;
                    }
                }
            }
            dataSet = null;
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        return dataSet;
    }

    // For select operation that returns a DataSet.
    // Returns stored procedure result value or method result value as output according to the isResultStoredProcedureResult parameter.
    // PROC: Stored Procedure.
    public DataSet PROC_SelectAdapter(string procedureName, out int result, bool isResultProcedureResult)
    {
        result = unsuccessful;
        DataSet dataSet;
        SqlConnection temporaryConnection = null;
        try
        {
            if (connectionOpenedOutside == true)
            {
                temporaryConnection = connection;
                dataAdapter.SelectCommand.Connection = temporaryConnection;
            }
            dataSet = new DataSet();
            dataAdapter.SelectCommand.CommandText = procedureName;
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        dataAdapter.SelectCommand.Parameters.Add(parameter);
                }
            }
            if (isResultProcedureResult)
            {
                SqlParameter procedureResultParameter = new SqlParameter("@procedureResult", SqlDbType.Int);
                procedureResultParameter.Direction = ParameterDirection.ReturnValue;
                dataAdapter.SelectCommand.Parameters.Add(procedureResultParameter);
            }
            dataAdapter.Fill(dataSet);
            if (isResultProcedureResult)
                result = (int)dataAdapter.SelectCommand.Parameters["@procedureResult"].Value;
            else
                result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            if (connectionOpenedOutside == true)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connectionOpenedOutside = false;
                    }
                }
            }
            dataSet = null;
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        return dataSet;
    }

    // For select operation that returns a scalar value.
    // SQL: Not Stored Procedure.
    public object SQL_SelectScalar(string sql, out int result)
    {
        result = mistake;
        object obj = null;
        if (sql.ToLower().Contains("select"))
        {
            result = unsuccessful;
            try
            {
                command.CommandText = sql;
                command.CommandType = CommandType.Text;
                command.Parameters.Clear();
                if (parameters != null)
                {
                    if (parameters.Length > 0)
                    {
                        foreach (SqlParameter parameter in parameters)
                            command.Parameters.Add(parameter);
                    }
                }
                if (connectionOpenedOutside == false)
                    connection.Open();
                obj = command.ExecuteScalar();
                result = successful;
                createParametersCalled = false;
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                int writeToLogFileResult;
                if (keepLog == true)
                    writeToLogFileResult = writeToLogFileForQuery(sql, message);
                result = exception;
            }
            finally
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        if (connectionOpenedOutside == false || result == exception)
                        {
                            connection.Close();
                            if (result == exception)
                                connectionOpenedOutside = false;
                        }
                    }
                }
            }
        }
        return obj;
    }

    // For select operation that returns a scalar value.
    // Returns no stored procedure result value as output.
    // PROC: Stored Procedure.
    public object PROC_SelectScalar(string procedureName, out int result)
    {
        result = unsuccessful;
        object obj = null;
        try
        {
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
            }
            if (connectionOpenedOutside == false)
                connection.Open();
            obj = command.ExecuteScalar();
            result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        finally
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    if (connectionOpenedOutside == false || result == exception)
                    {
                        connection.Close();
                        if (result == exception)
                            connectionOpenedOutside = false;
                    }
                }
            }
        }
        return obj;
    }

    // For select operation that returns a scalar value.
    // Returns stored procedure result value or method result value as output according to the isResultStoredProcedureResult parameter.
    // PROC: Stored Procedure.
    public object PROC_SelectScalar(string procedureName, out int result, bool isResultProcedureResult)
    {
        result = unsuccessful;
        object obj = null;
        try
        {
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
            }
            if (isResultProcedureResult)
            {
                SqlParameter procedureResultParameter = new SqlParameter("@procedureResult", SqlDbType.Int);
                procedureResultParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(procedureResultParameter);
            }
            if (connectionOpenedOutside == false)
                connection.Open();
            obj = command.ExecuteScalar();
            if (isResultProcedureResult)
                result = (int)command.Parameters["@procedureResult"].Value;
            else
                result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        finally
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    if (connectionOpenedOutside == false || result == exception)
                    {
                        connection.Close();
                        if (result == exception)
                            connectionOpenedOutside = false;
                    }
                }
            }
        }
        return obj;
    }

    // For select operation using a DataReader. This method returns a DataTable.
    // SQL: Not Stored Procedure.
    public DataTable SQL_SelectReader(string sql, out int result)
    {
        result = unsuccessful;
        SqlDataReader dataReader = null;
        DataTable dataTable = new DataTable();
        DataTable schemaTable;
        DataRow newDataRow;
        DataColumn dataColumn;
        ArrayList columnList = new ArrayList();
        string columnName;
        try
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            command.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
            }
            if (connectionOpenedOutside == false)
                connection.Open();
            dataReader = command.ExecuteReader();
            schemaTable = dataReader.GetSchemaTable();
            if (schemaTable != null)
            {
                foreach (DataRow dataRow in schemaTable.Rows)
                {
                    columnName = System.Convert.ToString(dataRow["ColumnName"]);
                    dataColumn = new DataColumn(columnName, (Type)(dataRow["DataType"]));
                    dataColumn.Unique = (bool)(dataRow["IsUnique"]);
                    dataColumn.AllowDBNull = (bool)(dataRow["AllowDBNull"]);
                    dataColumn.AutoIncrement = (bool)(dataRow["IsAutoIncrement"]);
                    columnList.Add(dataColumn);
                    dataTable.Columns.Add(dataColumn);
                }
            }
            while (dataReader.Read())
            {
                newDataRow = dataTable.NewRow();
                for (int i = 0; i <= columnList.Count - 1; i++)
                    newDataRow[((DataColumn)columnList[i])] = dataReader[i];
                dataTable.Rows.Add(newDataRow);
            }
            dataTable.AcceptChanges();
            result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            dataTable = null;
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(sql, message);
            result = exception;
        }
        finally
        {
            if (dataReader != null)
            {
                if (dataReader.IsClosed == false)
                {
                    dataReader.Close();
                }
            }
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    if (connectionOpenedOutside == false || result == exception)
                    {
                        connection.Close();
                        if (result == exception)
                            connectionOpenedOutside = false;
                    }
                }
            }
        }
        return dataTable;
    }

    // For select operation using a DataReader. This method returns a DataTable.
    // Returns no stored procedure result value as output.
    // PROC: Stored Procedure.
    public DataTable PROC_SelectReader(string procedureName, out int result)
    {
        result = unsuccessful;
        SqlDataReader dataReader = null;
        DataTable dataTable = new DataTable();
        DataTable schemaTable;
        DataRow newDataRow;
        DataColumn dataColumn;
        ArrayList columnList = new ArrayList();
        string columnName;
        try
        {
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
            }
            if (connectionOpenedOutside == false)
                connection.Open();
            dataReader = command.ExecuteReader();
            schemaTable = dataReader.GetSchemaTable();
            if (schemaTable != null)
            {
                foreach (DataRow dataRow in schemaTable.Rows)
                {
                    columnName = System.Convert.ToString(dataRow["ColumnName"]);
                    dataColumn = new DataColumn(columnName, (Type)(dataRow["DataType"]));
                    dataColumn.Unique = (bool)(dataRow["IsUnique"]);
                    dataColumn.AllowDBNull = (bool)(dataRow["AllowDBNull"]);
                    dataColumn.AutoIncrement = (bool)(dataRow["IsAutoIncrement"]);
                    columnList.Add(dataColumn);
                    dataTable.Columns.Add(dataColumn);
                }
            }
            while (dataReader.Read())
            {
                newDataRow = dataTable.NewRow();
                for (int i = 0; i <= columnList.Count - 1; i++)
                    newDataRow[((DataColumn)columnList[i])] = dataReader[i];
                dataTable.Rows.Add(newDataRow);
            }
            dataTable.AcceptChanges();
            result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            dataTable = null;
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        finally
        {
            if (dataReader != null)
            {
                if (dataReader.IsClosed == false)
                {
                    dataReader.Close();
                }
            }
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    if (connectionOpenedOutside == false || result == exception)
                    {
                        connection.Close();
                        if (result == exception)
                            connectionOpenedOutside = false;
                    }
                }
            }
        }
        return dataTable;
    }

    // For select operation using a DataReader. This method returns a DataTable.
    // Returns stored procedure result value or method result value as output according to the isResultStoredProcedureResult parameter.
    // PROC: Stored Procedure.
    public DataTable PROC_SelectReader(string procedureName, out int result, bool isResultProcedureResult)
    {
        result = unsuccessful;
        SqlDataReader dataReader = null;
        DataTable dataTable = new DataTable();
        DataTable schemaTable;
        DataRow newDataRow;
        DataColumn dataColumn;
        ArrayList columnList = new ArrayList();
        string columnName;
        try
        {
            command.CommandText = procedureName;
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            if (parameters != null)
            {
                if (parameters.Length > 0)
                {
                    foreach (SqlParameter parameter in parameters)
                        command.Parameters.Add(parameter);
                }
            }
            if (isResultProcedureResult)
            {
                SqlParameter procedureResultParameter = new SqlParameter("@procedureResult", SqlDbType.Int);
                procedureResultParameter.Direction = ParameterDirection.ReturnValue;
                command.Parameters.Add(procedureResultParameter);
            }
            if (connectionOpenedOutside == false)
                connection.Open();
            dataReader = command.ExecuteReader();
            schemaTable = dataReader.GetSchemaTable();
            if (schemaTable != null)
            {
                foreach (DataRow dataRow in schemaTable.Rows)
                {
                    columnName = System.Convert.ToString(dataRow["ColumnName"]);
                    dataColumn = new DataColumn(columnName, (Type)(dataRow["DataType"]));
                    dataColumn.Unique = (bool)(dataRow["IsUnique"]);
                    dataColumn.AllowDBNull = (bool)(dataRow["AllowDBNull"]);
                    dataColumn.AutoIncrement = (bool)(dataRow["IsAutoIncrement"]);
                    columnList.Add(dataColumn);
                    dataTable.Columns.Add(dataColumn);
                }
            }
            while (dataReader.Read())
            {
                newDataRow = dataTable.NewRow();
                for (int i = 0; i <= columnList.Count - 1; i++)
                    newDataRow[((DataColumn)columnList[i])] = dataReader[i];
                dataTable.Rows.Add(newDataRow);
            }
            dataTable.AcceptChanges();
            if (isResultProcedureResult)
                result = (int)command.Parameters["@procedureResult"].Value;
            else
                result = successful;
            createParametersCalled = false;
        }
        catch (Exception exc)
        {
            dataTable = null;
            string message = exc.Message;
            int writeToLogFileResult;
            if (keepLog == true)
                writeToLogFileResult = writeToLogFileForQuery(procedureName, message);
            result = exception;
        }
        finally
        {
            if (dataReader != null)
            {
                if (dataReader.IsClosed == false)
                {
                    dataReader.Close();
                }
            }
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    if (connectionOpenedOutside == false || result == exception)
                    {
                        connection.Close();
                        if (result == exception)
                            connectionOpenedOutside = false;
                    }
                }
            }
        }
        return dataTable;
    }

    // Creates the empty SQL command list to use with a transaction.
    // For insert, update and delete operations.
    // SQL: Not Stored Procedure.
    public int SQL_CreateCommands(int commandCount)
    {
        int result = mistake;
        if (commandCount > 0)
        {
            result = unsuccessful;
            try
            {
                commands = new SqlCommand[commandCount];
                commandIndex = 0;
                createCommandsCalled = true;
                result = successful;
            }
            catch (Exception exc)
            {
                if (connection != null)
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                        connectionOpenedOutside = false;
                    }
                }
                string message = exc.Message;
                result = exception;
            }
        }
        return result;
    }

    // Adds the command to the SQL command list to use with a transaction.
    // SQL_CreateCommands method must be called before.
    // For insert, update and delete operations.
    // SQL: Not Stored Procedure.
    public int SQL_AddCommand(string sql)
    {
        int result = mistake;
        if (createCommandsCalled)
        {
            if (sql.ToLower(new System.Globalization.CultureInfo("en-US", false)).Contains("insert") || sql.ToLower().Contains("update") || sql.ToLower().Contains("delete"))
            {
                result = unsuccessful;
                try
                {
                    int commandTimeout = command.CommandTimeout;
                    command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandTimeout = commandTimeout;
                    command.CommandText = sql;
                    command.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        if (parameters.Length > 0)
                        {
                            foreach (SqlParameter parameter in parameters)
                                command.Parameters.Add(parameter);
                        }
                    }
                    commands[commandIndex++] = command;
                    result = successful;
                    createParametersCalled = false;
                }
                catch (Exception exc)
                {
                    if (connection != null)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                            connectionOpenedOutside = false;
                        }
                    }
                    string message = exc.Message;
                    result = exception;
                }
            }
        }
        return result;
    }

    // For sequential queries, a transaction is used.
    // SQL_CreateCommands and SQL_AddCommand methods must be called before.
    // For insert, update and delete operations.
    // SQL: Not Stored Procedure.
    public int SQL_Transaction()
    {
        int result = mistake;
        if (commands != null)
        {
            if (commands.Length > 0)
            {
                result = unsuccessful;
                transaction = null;
                try
                {
                    if (connectionOpenedOutside == false)
                        connection.Open();
                    transaction = connection.BeginTransaction();
                    result = successful;
                }
                catch (Exception exc)
                {
                    if (connection != null)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                            connectionOpenedOutside = false;
                        }
                    }
                    string connectionString;
                    if (connection != null)
                    {
                        if (!connection.ConnectionString.Trim().Equals(""))
                            connectionString = connection.ConnectionString;
                        else
                            connectionString = "NO CONNECTION STRING";
                    }
                    else
                    {
                        connectionString = "NO CONNECTION STRING";
                    }
                    string message = exc.Message;
                    int writeToLogFileResult;
                    if (keepLog == true)
                        writeToLogFileResult = writeToLogFileForConnection(connectionString, message);
                    result = exception;
                }
                if (result == successful)
                {
                    result = unsuccessful;
                    int effectedRowCount = -99;
                    int index = -1;
                    try
                    {
                        for (int i = 0; i < commands.Length; i++)
                        {
                            index = i;
                            commands[i].Transaction = transaction;
                            effectedRowCount = commands[i].ExecuteNonQuery();
                        }
                        transaction.Commit();
                        result = successful;
                        createCommandsCalled = false;
                    }
                    catch (Exception exc)
                    {
                        transaction.Rollback();
                        string message = exc.Message;
                        int writeToLogFileResult;
                        if (keepLog == true)
                        {
                            if (index > -1)
                                writeToLogFileResult = writeToLogFileForQuery(commands[index].CommandText, message);
                            else
                                writeToLogFileResult = writeToLogFileForQuery("NO COMMANDS", message);
                        }
                        result = exception;
                    }
                    finally
                    {
                        if (connection != null)
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                if (connectionOpenedOutside == false || result == exception)
                                {
                                    connection.Close();
                                    if (result == exception)
                                        connectionOpenedOutside = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        return result;
    }

    // Clears the parameter list created by createParameters and addParameter functions.
    public int clearParameters()
    {
        int result = unsuccessful;
        try
        {
            if (parameters != null)
            {
                parameters = null;
                createParametersCalled = false;
                result = successful;
            }
        }
        catch (Exception exc)
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connectionOpenedOutside = false;
                }
            }
            string message = exc.Message;
            result = exception;
        }
        return result;
    }

    // Clears the command list created by SQL_CreateCommands and SQL_AddCommand functions.
    public int clearCommands()
    {
        int result = unsuccessful;
        try
        {
            if (commands != null)
            {
                commands = null;
                createCommandsCalled = false;
                result = successful;
            }
        }
        catch (Exception exc)
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                    connectionOpenedOutside = false;
                }
            }
            string message = exc.Message;
            result = exception;
        }
        return result;
    }
    #endregion
}
