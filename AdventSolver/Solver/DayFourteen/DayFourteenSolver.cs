using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventSolver.Solver.DayFourteen
{
    [AdventSolver(14)]
    public class DayFourteenSolver : SolverBase, IAdventSolver
    {
        public DayFourteenSolver() : base("Data\\Day14.txt")
        {
        }

        public void Solve()
        {
            var input = GetDataInput()
                .ToArray();

            var memoryVOne = new Dictionary<ulong, ulong>();
            var memoryVTwo = new Dictionary<ulong, ulong>();
            byte[] mask = new byte[36];

            foreach (string val in input)
            {
                if (val.StartsWith("mask"))
                {
                    mask = Convert(val);
                    continue;
                }

                var dataInput = val
                    .Replace("mem[", "")
                    .Replace("]", "")
                    .Split(" = ");

                ulong inputValue = ulong.Parse(dataInput[1]);
                ulong memoryAddress = ulong.Parse(dataInput[0]);
                ulong result = MaskData(inputValue, mask);
                
                if (!memoryVOne.ContainsKey(memoryAddress))
                    memoryVOne.Add(memoryAddress, 0);
                memoryVOne[memoryAddress] = result;

                foreach (ulong address in MaskMemoryAddress(memoryAddress, mask))
                {
                    if (!memoryVTwo.ContainsKey(address))
                        memoryVTwo.Add(address, 0);
                    memoryVTwo[address] = inputValue;
                }
            }

            long sum = memoryVOne.Values.Sum(l => (long)l);
            Console.WriteLine($"{sum} is the sum of all entries using decoder V1");
            sum = memoryVTwo.Values.Sum(l => (long)l);
            Console.WriteLine($"{sum} is the sum of all entries using decoder V2");
        }

        private static byte[] Convert(string mask)
        {
            byte[] maskArr = new byte[36];
            string toMaskString = mask.Replace("mask = ", "");

            for (int i = 0; i < toMaskString.Length; i++)
            {
                int pos = toMaskString.Length - 1 - i;
                maskArr[pos] = (byte)(toMaskString[i] == 'X' ? 3 : toMaskString[i] == '0' ? 0 : 1);
            }

            return maskArr;
        }

        private static ulong Convert(BitArray bitArray)
        {
            var array = new byte[8];
            bitArray.CopyTo(array, 0);
            return BitConverter.ToUInt64(array, 0);
        }

        private static ulong MaskData(ulong data, byte[] mask)
        {
            var bytes = BitConverter.GetBytes(data);
            BitArray outputData = new BitArray(bytes);
            for (int i = 0; i < mask.Length; i++)
            {
                byte b = mask[i];
                if (b == 0)
                    outputData[i] = false;
                if (b == 1)
                    outputData[i] = true;
            }

            return Convert(outputData);
        }

        private static IEnumerable<ulong> MaskMemoryAddress(ulong data, byte[] mask)
        {
            List<BitArray> values = new List<BitArray>();

            var bytes = BitConverter.GetBytes(data);
            BitArray outputData = new BitArray(bytes);
            values.Add(outputData);

            for (int i = 0; i < mask.Length; i++)
            {
                byte b = mask[i];
                if (b == 0)
                    continue;
                if (b == 1)
                {
                    values.ForEach(arr => arr[i] = true);
                    continue;
                }

                var bitArrays = values
                    .Select(arr => (BitArray)arr.Clone())
                    .ToList();
                values.ForEach(arr => arr[i] = true);
                bitArrays.ForEach(arr => arr[i] = false);
                values.AddRange(bitArrays);
            }

            return values.Select(Convert);
        }

    }
}