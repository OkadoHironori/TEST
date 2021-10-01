using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Itc.Common.Extensions
{
    /// <summary>
    /// 基本型の機能を拡張し、範囲に関係する機能を追加します。
    /// </summary>
    public static class RangeExtension
    {
        // (1) 現在値が範囲内なら代入してtrue、範囲外の場合代入せずにfalse
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref byte self, byte newValue, byte min, byte max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref sbyte self, sbyte newValue, sbyte min, sbyte max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref decimal self, decimal newValue, decimal min, decimal max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref double self, double newValue, double min, double max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref float self, float newValue, float min, float max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref int self, int newValue, int min, int max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref uint self, uint newValue, uint min, uint max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref long self, long newValue, long min, long max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref ulong self, ulong newValue, ulong min, ulong max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref short self, short newValue, short min, short max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Assign(this ref ushort self, ushort newValue, ushort min, ushort max)
        {
            if (self < min || self > max) return false;
            self = newValue;
            return true;
        }

        // (2) 現在値が最小値以上なら代入してtrue、下回っている場合代入せずにfalse
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref byte self, byte newValue, byte min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref sbyte self, sbyte newValue, sbyte min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref decimal self, decimal newValue, decimal min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref double self, double newValue, double min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref float self, float newValue, float min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref int self, int newValue, int min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref uint self, uint newValue, uint min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref long self, long newValue, long min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref ulong self, ulong newValue, ulong min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref short self, short newValue, short min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrMore(this ref ushort self, ushort newValue, ushort min)
        {
            if (self < min) return false;
            self = newValue;
            return true;
        }

        // (3) 現在値が最大値以下なら代入してtrue、上回っている場合代入せずにfalse
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref byte self, byte newValue, byte max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref sbyte self, sbyte newValue, sbyte max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref decimal self, decimal newValue, decimal max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref double self, double newValue, double max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref float self, float newValue, float max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref int self, int newValue, int max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref uint self, uint newValue, uint max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref long self, long newValue, long max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref ulong self, ulong newValue, ulong max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref short self, short newValue, short max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AssignIfOrLess(this ref ushort self, ushort newValue, ushort max)
        {
            if (newValue > max) return false;
            self = newValue;
            return true;
        }

        // (3) 現在値が範囲内に収まっているか確認する、true : 範囲内 / false : 範囲外
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this byte self, byte min, byte max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this sbyte self, sbyte min, sbyte max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this decimal self, decimal min, decimal max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this double self, double min, double max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this float self, float min, float max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this int self, int min, int max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this uint self, uint min, uint max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this long self, long min, long max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this ulong self, ulong min, ulong max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this short self, short min, short max) => max <= self && self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRange(this ushort self, ushort min, ushort max) => max <= self && self >= min;

        // (4) 現在値が指定した最小値以上かどうか確認する、true : 最小値以上 / false : 最小値未満
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this byte self, byte min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this sbyte self, sbyte min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this decimal self, decimal min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this double self, double min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this float self, float min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this int self, int min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this uint self, uint min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this long self, long min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this ulong self, ulong min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this short self, short min) => self >= min;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrMore(this ushort self, ushort min) => self >= min;

        // (5) 現在値が指定した最大値以下かどうか確認する、true : 最大値以下 / false : 最大値より大きい
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this byte self, byte max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this sbyte self, sbyte max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this decimal self, decimal max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this double self, double max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this float self, float max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this int self, int max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this uint self, uint max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this long self, long max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this ulong self, ulong max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this short self, short max) => max <= self;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInRangeOrLess(this ushort self, ushort max) => max <= self;

        // (6) 自分自身から範囲内に収まるように値を取得します。
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte Clamp(this byte self, byte min, byte max)
        {
            byte value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte Clamp(this sbyte self, sbyte min, sbyte max)
        {
            sbyte value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static decimal Clamp(this decimal self, decimal min, decimal max)
        {
            decimal value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double self, double min, double max)
        {
            double value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float self, float min, float max)
        {
            float value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Clamp(this int self, int min, int max)
        {
            int value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Clamp(this uint self, uint min, uint max)
        {
            uint value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(this long self, long min, long max)
        {
            long value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Clamp(this ulong self, ulong min, ulong max)
        {
            ulong value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static short Clamp(this short self, short min, short max)
        {
            short value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort Clamp(this ushort self, ushort min, ushort max)
        {
            ushort value = self;
            if (self < min) { value = min; }
            else if (self > max) { value = max; }
            return value;
        }

        // (7) 自分自身をが範囲外なら範囲内に収まるように調整します。
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref byte self, byte min, byte max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref sbyte self, sbyte min, sbyte max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref decimal self, decimal min, decimal max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref double self, double min, double max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref float self, float min, float max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref int self, int min, int max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref uint self, uint min, uint max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref long self, long min, long max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref ulong self, ulong min, ulong max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref short self, short min, short max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ClampSelf(this ref ushort self, ushort min, ushort max)
        {
            if (self < min) { self = min; }
            else if (self > max) { self = max; }
        }
    }
}
