using System;

public class DatabasePhenomenon()
{
	public string phenomenonName;
	private Dictionary<string, string> phenomenons;
	
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

public class DatabasePhenomenons()
{
	public DatabasePhenomenon[] databasePhenomenons;
}