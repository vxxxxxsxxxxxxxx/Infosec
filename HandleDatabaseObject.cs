using System;

/// <summary>
/// Summary description for Class1
/// </summary>
/// 
public class DatabaseObject()
{
	private Dictionary<string, string> phenomenon;
	
	public Dictionary<string, string> getPhenomenon()
	{
		return this.phenomenon;
	}
	
	public setPhenomenon(Dictionary<string, string> inputPhenomenon)
	{
		this.phenomenon = inputPhenomenon;
	}
	
	public string getFieldContentFromPhenomenon(string inputFieldName)
	{
		return this.phenomenon.TryGetValue(inputFieldName);
	}
	
	public void setFieldContentFromPhenomenon(string inputFieldName, string inputFieldContent)
	{
		this.phenomenon[inputFieldName] = inputFieldContent;
	}
}

public class DatabaseObjects()
{
	public DatabaseObject[] databaseObjects;
}