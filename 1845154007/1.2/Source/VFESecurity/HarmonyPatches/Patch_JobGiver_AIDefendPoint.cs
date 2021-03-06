using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using RimWorld;
using HarmonyLib;

namespace VFESecurity
{

    public static class Patch_JobGiver_AIDefendPoint
    {

        [HarmonyPatch(typeof(JobGiver_AIDefendPoint), "TryFindShootingPosition")]
        public static class TryFindShootingPosition
        {

            public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                #if DEBUG
                    Log.Message("Transpiler start: JobGiver_AIDefendPoint.TryFindShootingPosition (1 match)");
                #endif

                var instructionList = instructions.ToList();

                var rangeInfo = AccessTools.Field(typeof(VerbProperties), nameof(VerbProperties.range));

                var adjustedRangeInfo = AccessTools.Method(typeof(TrenchUtility), nameof(TrenchUtility.FinalAdjustedRangeFromTerrain), new Type[] { typeof(float), typeof(Verb), typeof(Thing) });

                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    if (instruction.opcode == OpCodes.Ldfld && instruction.OperandIs(rangeInfo))
                    {
                        #if DEBUG
                            Log.Message("JobGiver_AIDefendPoint.TryFindShootingPosition match 1 of 1");
                        #endif

                        yield return instruction; // verb.verbProps.range
                        yield return new CodeInstruction(OpCodes.Ldloc_1); // verb
                        yield return new CodeInstruction(OpCodes.Ldarg_1); // pawn
                        instruction = new CodeInstruction(OpCodes.Call, adjustedRangeInfo); // AdjustedRange(verb.verbProps.range, verb, pawn)
                    }

                    yield return instruction;
                }
            }

        }

    }

}
