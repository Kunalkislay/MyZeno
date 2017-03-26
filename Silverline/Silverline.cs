using System;

public class Silverline
{
    SqlConnection connection;
    List<Activities> activities;
    public Silverline()
	{
        string strConn = "Server=tcp:ioeventnonprod.database.windows.net,1433;Initial Catalog=ioeventqa;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        connection = new SqlConnection(strConn);

    }
    public List<Activities> getAllActivities()
    {
        connection.Open();
        SqlCommand selectCommand = new SqlCommand("", connection);
        selectCommand.CommandType = CommandType.Text;
        selectCommand.CommandText = @"select TOP 5  Id,EventID, TypeLIC,Description, StartDate, EndDate,statusLIC, OwnedBy from dbo.EMS_ACTIVITY";
        SqlDataReader reader = selectCommand.ExecuteReader();
        while (reader.Read())
        {
            // show data
            Activities activity = new Activities();
            activity.ID = reader.GetString(0);
            activity.EventID = reader.GetString(1);
            activity.Type = reader.GetString(2);
            activity.Description = reader.GetString(3);
            activity.Status = reader.GetString(6);
            activities.add(activity);
        }
        reader.Close();
        return activities;

    }
    
}
