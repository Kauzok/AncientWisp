﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace AncientWisp.Helpers
{
    class DumpPrefab
    {
		public static void GetPrefabInfo(GameObject go)
		{
			string text = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Bodys/";
			string text2 = "";
			text2 = text2 + text + "\n";
			types = new List<Type>();
			try
			{
				text2 += "create Directory\n";
				Directory.CreateDirectory(text);
				text2 += "for each\n";
				string text3;
				Component[] components = go.GetComponents<Component>();
				text3 = "";
				text3 = text3 + go.name + "\n";
				text3 += OutputComponents(components, ">");
				text3 += GetChildren(go.transform, ">");
				File.WriteAllText(text + go.name + ".txt", text3);
				text2 += "types\n";
				text3 = "";
				text3 += "\n\ntypes\n\n";
				for (int j = 0; j < types.Count; j++)
				{
					MemberInfo[] members = types[j].GetMembers();
					text3 = text3 + types[j].Name + "\n";
					for (int k = 0; k < members.Length; k++)
					{
						bool flag = !(members[k].DeclaringType.Name == "MonoBehaviour") && !(members[k].DeclaringType.Name == "Component") && !(members[k].DeclaringType.Name == "Behaviour") && !(members[k].DeclaringType.Name == "Object");
						bool flag2 = flag;
						if (flag2)
						{
							text3 = string.Concat(new string[]
							{
								text3,
								"> ",
								members[k].MemberType.ToString(),
								": ",
								members[k].Name,
								"\n"
							});
						}
					}
				}
				File.WriteAllText(text + "types.txt", text3);
			}
			catch (Exception ex)
			{
				text2 += ex.ToString();
			}
			File.WriteAllText(text + "log.txt", text2);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002308 File Offset: 0x00000508
		public void Update()
		{
		}

		// Token: 0x06000003 RID: 3 RVA: 0x0000230C File Offset: 0x0000050C
		public static string OutputComponents(Component[] components, string delimi)
		{
			string text = "";
			for (int i = 0; i < components.Length; i++)
			{
				string fullName = components[i].GetType().FullName;
				bool flag = !(fullName == "UnityEngine.Transform");
				string text2;
				if (flag)
				{
					Type type = components[i].GetType();
					text2 = type.FullName + "\n";
					foreach (FieldInfo fieldInfo in type.GetFields())
					{
						text2 = string.Concat(new object[]
						{
							text2,
							delimi,
							"v ",
							fieldInfo.Name,
							" = ",
							fieldInfo.GetValue(components[i]),
							"\n"
						});
					}
				}
				else
				{
					Transform transform = (Transform)components[i];
					text2 = string.Concat(new string[]
					{
						"transform = p:",
						transform.localPosition.ToString(),
						" r:",
						transform.eulerAngles.ToString(),
						" s:",
						transform.localScale.ToString(),
						"\n"
					});
				}
				text = string.Concat(new string[]
				{
					text,
					"\n",
					delimi,
					" ",
					text2
				});
				bool flag2 = !types.Contains(components[i].GetType());
				bool flag3 = flag2;
				if (flag3)
				{
					types.Add(components[i].GetType());
				}
			}
			return text;
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000024D4 File Offset: 0x000006D4
		public static string GetChildren(Transform transform, string delimi)
		{
			string text = "";
			for (int i = 0; i < transform.childCount; i++)
			{
				GameObject gameObject = transform.GetChild(i).gameObject;
				text = string.Concat(new string[]
				{
					text,
					delimi,
					"c ",
					gameObject.name,
					"\n"
				});
				Component[] components = gameObject.GetComponents<Component>();
				text += OutputComponents(components, delimi + ">");
				text += GetChildren(transform.GetChild(i), delimi + ">");
			}
			return text;
		}

		// Token: 0x04000001 RID: 1
		public static List<Type> types;
	}
}
