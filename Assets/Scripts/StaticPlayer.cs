using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticPlayer
{
	public static string Email { get; set; }
	public static string UserName { get; set; }
	public static int PlayerID { get; set; }
	public static int ARSkinID { get; set; }
	public static int HandgunSkinID { get; set; }

	public static void ResetPlayerInfo()
	{
		Email = "";
		UserName = "";
		PlayerID = 0;
		ARSkinID = 0;
		HandgunSkinID = 0;
	}
	
}
