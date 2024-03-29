﻿// ReSharper disable CheckNamespace
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable IdentifierTypo

namespace Beyond.Utilities;

public static class ConnectionStringUtility
{
    public static string GetFirebird(string database, string userId, string password,
        string dataSource = "localhost", int port = 3050, int dialect = 3, string charset = "UTF8",
        string role = "", int connectionLifeTime = 15, bool pooling = true, int minSizePool = 0,
        int maxSizePool = 50, int packetSize = 8192, byte serverType = 0, string more = "")
    {
        var pool = pooling ? "true" : "false";
        return
            $"User={userId};Password={password};Database={database};DataSource={dataSource};Port = {port}; Dialect = {dialect}; Charset = {charset}; Role ={role}; Connection lifetime = {connectionLifeTime}; Pooling = {pool};MinPoolSize = {minSizePool}; MaxPoolSize = {maxSizePool}; Packet Size = {packetSize}; ServerType = {serverType};{more}";
    }

    public static string GetMySql(string server, string database, string userId, string password, string more = "")
    {
        return $"Server={server};Database={database};Uid={userId};Pwd={password};{more}";
    }

    public static string GetMySqlWithPort(string server, string port, string database, string userId, string password, string more = "")
    {
        return $"Server={server};Port={port};Database={database};Uid={userId};Pwd={password};{more}";
    }

    public static string GetOracle(string database, string more = "")
    {
        return $"Data Source={database};Integrated Security=yes;{more}";
    }

    public static string GetOracle(string database, string userId, string password, string more = "")
    {
        return $"Data Source={database};User Id={userId};Password={password};Integrated Security = no;{more}";
    }

    public static string GetOracle(string host, string port, string serviceName, string userId, string password, string more = "")
    {
        return
            $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={serviceName})));User Id={userId};Password={password};{more}";
    }

    public static string GetPostgreSql(string server, string database, string userId, string password, int port = 5432,
        int commandTimeout = 20, int timeout = 15, string more = "")
    {
        return
            $"Server={server};Port={port};Database={database};User Id={userId};Password = {password};CommandTimeout={commandTimeout};Timeout={timeout};{more}";
    }

    public static string GetPostgreSql(string server, string database, int port = 5432, string more = "")
    {
        return $"Server={server};Port={port};Database={database};Integrated Security=true;{more}";
    }

    public static string GetSqLite3(string database, bool useUtf16 = true, string more = "")
    {
        var utf = useUtf16 ? "True" : "False";
        return $"Data Source={database};Version=3;UseUTF16Encoding={utf};{more}";
    }

    public static string GetSqLite3(string database, string password, bool useUtf16 = true, string more = "")
    {
        var utf = useUtf16 ? "True" : "False";
        return $"Data Source={database};Version=3;Password={password};UseUTF16Encoding={utf};{more}";
    }

    public static string GetSqlServer(string server, string database, string userId, string password, string more = "")
    {
        return $"Server={server};Database={database};User Id={userId};Password={password};{more}";
    }

    public static string GetSqlServer(string server, string database, string more = "")
    {
        return $"Server={server};Database={database};Trusted_Connection=True;{more}";
    }

    public static string GetSqlServerWithInstance(string server, string instance, string database, string userId,
        string password, string more = "")
    {
        return $"Server={server}\\{instance};Database={database};User Id={userId};Password={password};{more}";
    }
}