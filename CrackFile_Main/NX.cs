using System;
using NXOpen;
using NXOpen.UF;
using static NXOpen.UIStyler.DialogItem;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

public class Program
{
	// class members
	private static Session theSession;
	private static UI theUI;
	private static UFSession theUfSession;
	public static Program theProgram;
	public static bool isDisposeCalled;

	//------------------------------------------------------------------------------
	// Constructor
	//------------------------------------------------------------------------------
	public Program()
	{
		try
		{
			theSession = Session.GetSession();
			theUI = UI.GetUI();
			theUfSession = UFSession.GetUFSession();
			isDisposeCalled = false;
		}
		catch (NXOpen.NXException ex)
		{
			// ---- Enter your exception handling code here -----
			// UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
		}
	}



	//------------------------------------------------------------------------------
	//  Explicit Activation
	//      This entry point is used to activate the application explicitly
	//------------------------------------------------------------------------------
	public static int Main(string[] args)
	{
		int retValue = 0;
		try
		{
			theProgram = new Program();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new CrackFile_Main.Mains());
			theProgram.Dispose();
		}
		catch (NXOpen.NXException ex)
		{
			// ---- Enter your exception handling code here -----
		}
		return retValue;
	}

	//------------------------------------------------------------------------------
	//  NX Startup
	//      This entry point activates the application at NX startup

	//Will work when complete path of the dll is provided to Environment Variable 
	//USER_STARTUP or USER_DEFAULT

	//OR

	//Will also work if dll is at folder named "startup" under any folder listed in the 
	//text file pointed to by the environment variable UGII_CUSTOM_DIRECTORY_FILE.
	//------------------------------------------------------------------------------
	public static int Startup()
	{
		int retValue = 0;
		try
		{
			theProgram = new Program();

			//TODO: Add your application code here 

		}
		catch (NXOpen.NXException ex)
		{
			// ---- Enter your exception handling code here -----
			// theUI.NXMessageBox.Show("UI Styler", NXMessageBox.DialogType.Error, ex.Message);
		}
		return retValue;
	}

	//------------------------------------------------------------------------------
	// Following method disposes all the class members
	//------------------------------------------------------------------------------
	public void Dispose()
	{
		try
		{
			if (isDisposeCalled == false)
			{
				//TODO: Add your application code here 
			}
			isDisposeCalled = true;
		}
		catch (NXOpen.NXException ex)
		{
			// ---- Enter your exception handling code here -----

		}
	}

	public static int GetUnloadOption(string arg)
	{
		//Unloads the image explicitly, via an unload dialog
		//return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

		//Unloads the image immediately after execution within NX
		return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

		//Unloads the image when the NX session terminates
		// return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
	}

}

