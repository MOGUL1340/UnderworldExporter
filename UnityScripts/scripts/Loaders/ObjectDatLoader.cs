﻿using UnityEngine;
using System.Collections;

public class ObjectDatLoader : Loader {

		public struct MeleeData
		{
				//0000   Int8   damage modifier for Slash attack
				//0001   Int8   damage modifier for Bash attack
				//0002   Int8   damage modifier for Stab attack
				//0003   3      unknown
				//0006   Int8   skill type (3: sword, 4: axe, 5: mace, 6: unarmed)
				//0007   Int8   durability	
			public short Slash;
			public short Bash;
			public short Stab;
			public short Skill;
			public short Durability;
		};

		public struct RangedData
		{

				//0000   Int16  unknown
				//bits 9-15: ammunition needed (+0x10)
				//0002   Int8   durability	
			public int ammo;
			public int durability;
		}

		public struct ArmourData
		{
				/*0000   Int8   protection
				0001   Int8   durability
				0002   Int8   unknown
				0003   Int8   category:
				00: shield
				01: body armour
				03: leggings
				04: gloves
				05: boots
				08: hat
				09: ring*/
				public short protection;
				public short durability;
				public short category;
		}

		public struct ContainerData
		{
				/** Containers table (0x0080-0x008f)

				0000   Int8   capacity in 0.1 stones
				0001   Int8   objects accepted; 0: runes, 1: arrows, 2: scrolls, 3: edibles, 0xFF: any
				0002   Int8   number of slots available?; 2: , -1: any
				*/

				public int capacity;
				public int objectsMask;
				public int slots;
		}

		public struct LightSourceData
		{
				//* Light source table (0x0090-0x009f)
				public int brightness;
				public int duration;
				//0000   Int8   light brightness (max. is 4; 0 means unlit)
				//0001   Int8   duration (00: doesn't go out, e.g. taper of sacrifice)
		}


		public struct CritterData
		{
				/*
00h 	1 	uint8 	Level 	Level of the creature.
01h 	3 	 ?? 	 ?? 	 ??
04h 	2 	uint16 	HitPoints 	Average hit points.
06h 	1 	uint8 	AttackPower 	Damage on attack.
07h 	1 	 ?? 	 ?? 	 ??
08h 	1 	uint8 	FluidAndRemains 	A combination of remains after death and the type of blood splatters this produces. Mask 0x0F is the splatter type, 0 for dust, 8 for red blood. Mask 0xF0 is the remains; Nothing = 0x00, RotwormCorpse = 0x20, Rubble = 0x40, WoodChips = 0x60, Bones = 0x80, GreenBloodPool = 0xA0, RedBloodPool = 0xC0, RedBloodPoolGiantSpider = 0xE0.
09h 	1 	uint8 	GeneralType 	An index into the strings on page 8, offset 370. This string is the generic name for the creature, like "a creature" for "a goblin" or "a rat" for "a giant rat".
0Ah 	1 	uint8 	Passiveness 	Relative passiveness. 255 will never take a swing at you, even if you kill them.
0Bh 	1 	 ?? 	 ?? 	 ??
0Ch 	1 	uint8 	MovementSpeed 	Speed of movement; 0 is immobile, maxes out at 12 for vampire bat.
0Dh 	2 	 ?? 	 ?? 	 ??
0Fh 	1 	uint8 	PoisonDamage 	Amount of poison damage this is capable of on attack.
10h 	1 	uint8 	Category 	Ethereal = 0x00 (Ethereal critters like ghosts, wisps, and shadow beasts), Humanoid = 0x01 (Humanlike non-thinking forms like lizardmen, trolls, ghouls, and mages), Flying = 0x02 (Flying critters like bats and imps), Swimming = 0x03 (Swimming critters like lurkers), Creeping = 0x04 (Creeping critters like rats and spiders), Crawling = 0x05 (Crawling critters like slugs, worms, reapers (!), and fire elementals (!!)), EarthGolem = 0x11 (Only used for the earth golem), Human = 0x51 (Humanlike thinking forms like goblins, skeletons, mountainmen, fighters, outcasts, and stone and metal golems).
11h 	1 	uint8 	EquipmentDamage 	Amount of equipment damage this is capable of on attack.
12h 	1 	 ?? 	 ?? 	 ??
13h 	9 	Probability[3] 	Probabilities 	Each has the form (uint16 value, uint8 percent). What this means is unknown.
1Ch 	12 	 ?? 	 ?? 	 ??
28h 	2 	uint16 	Experience 	Experience provided when killed.
2Ah 	5 	 ?? 	 ?? 	 ??
2Fh 	1 	uint8 	 ?? 	Always 73.
*/
				public int Level;
				public short AvgHit;//Is this defence?????
				public int AttackPower;
				public int Remains;
				public int Blood;
				public int GeneralType;
				public int Passive;
				public int Speed;
				public int Poison;
				public int Category;
				public int EquipDamage;
				public int ProbValue1;
				public int ProbPercent1;
				public int ProbValue2;
				public int ProbPercent2;
				public int ProbValue3;
				public int ProbPercent3;
				public int Exp;
		};


		public MeleeData[] weaponStats=new MeleeData[16];
		public RangedData[] rangedStats=new RangedData[16];
		public ArmourData[] armourStats=new ArmourData[32];
		public ContainerData[] containerStats=new ContainerData[16];
		public LightSourceData[] lightSourceStats=new LightSourceData[8];
		public CritterData[]  critterStats = new CritterData[64];

		public ObjectDatLoader()
		{
			char[] obj_dat;
			int add_ptr;
			if (DataLoader.ReadStreamFile(BasePath+"\\data\\objects.dat",out obj_dat))
			{
				add_ptr=2;
				int j=0;
				for (int i=0; i<16;i++)
				{
					weaponStats[j].Slash = (short)DataLoader.getValAtAddress(obj_dat, add_ptr, 8);
					weaponStats[j].Bash = (short)DataLoader.getValAtAddress(obj_dat, add_ptr+1, 8);
					weaponStats[j].Stab = (short)DataLoader.getValAtAddress(obj_dat, add_ptr+2, 8);
					weaponStats[j].Skill = (short)DataLoader.getValAtAddress(obj_dat, add_ptr+6, 8);
					weaponStats[j].Durability = (short)DataLoader.getValAtAddress(obj_dat, add_ptr+7, 8);
					add_ptr=add_ptr+8;	
					j++;
				}


				add_ptr=0x82;
				j=0;
				for (int i = 0; i < 16; i++)
				{//Ranged weapons
				//This is probably wrong!!
					rangedStats[j].ammo=0x10 + ((((int)DataLoader.getValAtAddress(obj_dat,  add_ptr, 16) >> 9) & 0x7F));
					rangedStats[j].durability= (int)DataLoader.getValAtAddress(obj_dat,  add_ptr + 2, 8);
					add_ptr = add_ptr + 3;
					j++;
				}

				add_ptr=0xb2;
				j=0;
				for (int i = 0; i < 32; i++)
				{
					armourStats[j].protection= (short)DataLoader.getValAtAddress(obj_dat, add_ptr, 8);
					armourStats[j].durability= (short)DataLoader.getValAtAddress(obj_dat, add_ptr + 1, 8);
					armourStats[j].category=(short)DataLoader.getValAtAddress(obj_dat, add_ptr + 3, 8);
					add_ptr = add_ptr + 4;
					j++;
				}


				add_ptr=0xd32;
				j=0;
				for (int i = 0; i < 16; i++)
				{
					containerStats[i].capacity =(int)DataLoader.getValAtAddress(obj_dat, add_ptr, 8);
					containerStats[i].objectsMask=(int)DataLoader.getValAtAddress(obj_dat, add_ptr+1, 8);
					containerStats[i].slots =(int)DataLoader.getValAtAddress(obj_dat, add_ptr+2, 8);
					add_ptr = add_ptr + 3;
					j++;
				}


				add_ptr=0xd62;
				j=0;
				for (int i = 0; i < 8; i++)
				{//Light sources
					lightSourceStats[j].duration = (int)DataLoader.getValAtAddress(obj_dat, add_ptr, 8);
					lightSourceStats[j].brightness =(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 1, 8);
					add_ptr = add_ptr + 2;
					j++;
				}

				add_ptr=0x132;
				j=0;
				for (int i = 0; i < 64; i++)
				{//Critters
					critterStats[j].Level=(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0, 8);//Level
					critterStats[j].AvgHit=(short)DataLoader.getValAtAddress(obj_dat, add_ptr + 4, 16);//Average Hitpoints
					critterStats[j].AttackPower= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 6, 8);//Attack power
					critterStats[j].Remains=(int)DataLoader.getValAtAddress(obj_dat,add_ptr + 8, 8) & 0xF0;//Remains body
					critterStats[j].Blood=(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 8, 8) & 0x0F;//Remains blood
					critterStats[j].GeneralType= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 9, 8);//General Type
					critterStats[j].Passive=(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0xA, 8);//Passiveness
					critterStats[j].Speed=(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0xC, 8);//Speed
					critterStats[j].Poison=(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0xF, 8);//Poison Damage
					critterStats[j].Category= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x10, 8);//Category
					critterStats[j].EquipDamage= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x11, 8);//Equipment damage
					critterStats[j].ProbValue1=(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x13, 16);//Probability1
					critterStats[j].ProbPercent1=(int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x15, 8);//Probab1
					critterStats[j].ProbValue2= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x16, 16);//Probability2
					critterStats[j].ProbPercent2= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x18, 8);//Probab2
					critterStats[j].ProbValue3= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x19, 16);//Probability3
					critterStats[j].ProbPercent3= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x1B, 8);//Probab3
					critterStats[j].Exp= (int)DataLoader.getValAtAddress(obj_dat, add_ptr + 0x28, 16);//Exp
					add_ptr = add_ptr + 48;
				j++;
				}

			}
		}
}
