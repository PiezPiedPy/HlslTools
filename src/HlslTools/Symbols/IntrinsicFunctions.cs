﻿using System.Collections.Generic;
using System.Linq;

namespace HlslTools.Symbols
{
    public static class IntrinsicFunctions
    {
        public static readonly IEnumerable<FunctionDeclarationSymbol> AllFunctions;

        static IntrinsicFunctions()
        {
            var allFunctions = new List<FunctionDeclarationSymbol>();

            // From https://msdn.microsoft.com/en-us/library/windows/desktop/ff471350(v=vs.85).aspx

            allFunctions.Add(Create0(
                "abort",
                "Submits an error message to the information queue and terminates the current draw or dispatch call being executed.",
                IntrinsicTypes.Void));

            allFunctions.AddRange(Create1(
                "abs", 
                "Returns the absolute value of the specified value.", 
                IntrinsicTypes.AllNumericTypes,
                "value", "The specified value"));

            allFunctions.AddRange(Create1(
                "acos", 
                "Returns the arccosine of the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value. Each component should be a floating-point value within the range of -1 to 1."));

            allFunctions.AddRange(Create1(
                "all", 
                "Determines if all components of the specified value are non-zero.", 
                IntrinsicTypes.AllNumericTypes,
                "value", "The specified value",
                IntrinsicTypes.Bool));

            allFunctions.Add(Create0(
                "AllMemoryBarrier", 
                "Blocks execution of all threads in a group until all memory accesses have been completed.", 
                IntrinsicTypes.Void));

            allFunctions.Add(Create0(
                "AllMemoryBarrierWithGroupSync", 
                "Blocks execution of all threads in a group until all memory accesses have been completed and all threads in the group have reached this call.", 
                IntrinsicTypes.Void));

            allFunctions.AddRange(Create1(
                "any", 
                "Determines if any components of the specified value are non-zero.", 
                IntrinsicTypes.AllNumericTypes,
                "value", "The specified value",
                IntrinsicTypes.Bool));

            allFunctions.AddRange(Create2(
                "asdouble", 
                "Reinterprets a cast value (two 32-bit values) into a double.",
                new [] { IntrinsicTypes.Uint },
                "lowbits", "The low 32-bit pattern of the input value.",
                "highbits", "The high 32-bit pattern of the input value.",
                IntrinsicTypes.Double));
            allFunctions.AddRange(Create2(
                "asdouble",
                "Reinterprets a cast value (two 32-bit values) into a double.",
                new[] { IntrinsicTypes.Uint2 },
                "lowbits", "The low 32-bit pattern of the input values.",
                "highbits", "The high 32-bit pattern of the input values.",
                IntrinsicTypes.Double2));

            allFunctions.AddRange(Create1(
                "atan", 
                "Returns the arctangent of the specified value.", 
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value"));

            allFunctions.AddRange(Create2(
                "atan2", 
                "Returns the arctangent of two values (x,y).", 
                IntrinsicTypes.AllFloatTypes,
                "y", "The y value.",
                "x", "The x value."));

            allFunctions.AddRange(Create1(
                "ceil",
                "Returns the smallest integer value that is greater than or equal to the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value"));

            allFunctions.AddRange(Create1(
                "CheckAccessFullyMapped",
                "Determines whether all values from a Sample, Gather, or Load operation accessed mapped tiles in a tiled resource.",
                new [] { IntrinsicTypes.Uint },
                "status", "The status value that is returned from a Sample, Gather, or Load operation. Because you can't access this status value directly, you need to pass it to CheckAccessFullyMapped.",
                IntrinsicTypes.Bool));

            allFunctions.AddRange(Create3(
                "clamp",
                "Clamps the specified value to the specified minimum and maximum range.",
                IntrinsicTypes.AllFloatTypes.Union(IntrinsicTypes.AllIntTypes).ToArray(),
                "value", "A value to clamp.",
                "min", "The specified minimum range.",
                "max", "The specified maximum range."));

            allFunctions.AddRange(Create1(
                "clip",
                "Discards the current pixel if the specified value is less than zero.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value"));

            allFunctions.AddRange(Create1(
                "cos",
                "Returns the cosine of the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value, in radians."));

            allFunctions.AddRange(Create1(
                "cosh",
                "Returns the hyperbolic cosine of the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value, in radians."));

            allFunctions.AddRange(Create1(
                "countbits",
                "Counts the number of bits (per component) in the input integer.",
                new [] { IntrinsicTypes.Uint }.Union(IntrinsicTypes.AllUintVectorTypes).ToArray(),
                "value", "The input value."));

            allFunctions.AddRange(Create2(
                "cross",
                "Returns the cross product of two floating-point, 3D vectors.",
                new[] { IntrinsicTypes.Float3 },
                "x", "The first floating-point, 3D vector.",
                "y", "The second floating-point, 3D vector."));

            allFunctions.AddRange(Create1(
                "D3DCOLORtoUBYTE4",
                "Converts a floating-point, 4D vector set by a D3DCOLOR to a UBYTE4.",
                new[] { IntrinsicTypes.Float4 },
                "value", "The floating-point vector4 to convert.",
                IntrinsicTypes.Int4));

            allFunctions.AddRange(Create1(
                "ddx",
                "Returns the partial derivative of the specified value with respect to the screen-space x-coordinate.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "ddx_coarse",
                "Computes a low precision partial derivative with respect to the screen-space x-coordinate.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "ddx_fine",
                "Computes a high precision partial derivative with respect to the screen-space x-coordinate.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "ddy",
                "Returns the partial derivative of the specified value with respect to the screen-space y-coordinate.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "ddy_coarse",
                "Computes a low precision partial derivative with respect to the screen-space y-coordinate.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "ddy_fine",
                "Computes a high precision partial derivative with respect to the screen-space y-coordinate.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "degrees",
                "Converts the specified value from radians to degrees.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "determinant",
                "Returns the determinant of the specified floating-point, square matrix.",
                IntrinsicTypes.AllFloatMatrixTypes,
                "value", "The specified value.",
                IntrinsicTypes.Float));

            allFunctions.Add(Create0(
                "DeviceMemoryBarrier",
                "Blocks execution of all threads in a group until all device memory accesses have been completed.",
                IntrinsicTypes.Void));

            allFunctions.Add(Create0(
                "DeviceMemoryBarrierWithGroupSync",
                "Blocks execution of all threads in a group until all device memory accesses have been completed and all threads in the group have reached this call.",
                IntrinsicTypes.Void));

            allFunctions.AddRange(Create2(
                "distance",
                "Returns a distance scalar between two vectors.",
                IntrinsicTypes.AllFloatVectorTypes,
                "x", "The first floating-point vector to compare.",
                "y", "The second floating-point vector to compare.",
                IntrinsicTypes.Float));

            allFunctions.AddRange(Create2(
               "dot",
               "Returns the dot product of two vectors.",
               IntrinsicTypes.AllFloatVectorTypes,
               "x", "The first vector.",
               "y", "The second vector.",
               IntrinsicTypes.Float));
            allFunctions.AddRange(Create2(
               "dot",
               "Returns the dot product of two vectors.",
               IntrinsicTypes.AllIntVectorTypes,
               "x", "The first vector.",
               "y", "The second vector.",
               IntrinsicTypes.Int));

            allFunctions.AddRange(Create2(
               "dst",
               "Calculates a distance vector.",
               new [] { IntrinsicTypes.Float4 },
               "x", "The first vector.",
               "y", "The second vector."));

            // TODO: errorf (requires string type and variadic arguments)

            allFunctions.AddRange(Create1(
               "EvaluateAttributeAtCentroid",
               "Evaluates at the pixel centroid.",
               IntrinsicTypes.AllNumericTypes,
               "value", "The input value."));

            allFunctions.AddRange(Create2(
               "EvaluateAttributeAtSample",
               "Evaluates at the indexed sample location.",
               IntrinsicTypes.AllNumericTypes,
               "value", "The input value.",
               "sampleIndex", "The sample location.",
               overrideParameterType2: IntrinsicTypes.Uint));

            allFunctions.AddRange(Create2(
               "EvaluateAttributeSnapped",
               "Evaluates at the pixel centroid with an offset.",
               IntrinsicTypes.AllNumericTypes,
               "value", "The input value.",
               "offset", "A 2D offset from the pixel center using a 16x16 grid.",
               overrideParameterType2: IntrinsicTypes.Int2));

            allFunctions.AddRange(Create1(
                "exp",
                "Returns the base-e exponential, or e^x, of the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "exp2",
                "Returns the base 2 exponential, or 2^x, of the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "f16tof32",
                "Converts the float16 stored in the low-half of the uint to a float.",
                new[] { IntrinsicTypes.Uint }.Union(IntrinsicTypes.AllUintVectorTypes).ToArray(),
                "value", "The input value.",
                new[] { IntrinsicTypes.Float }.Union(IntrinsicTypes.AllFloatVectorTypes).ToArray()));

            allFunctions.AddRange(Create1(
                "f32tof16",
                "Converts an input into a float16 type.",
                new[] { IntrinsicTypes.Float }.Union(IntrinsicTypes.AllFloatVectorTypes).ToArray(),
                "value", "The input value.",
                new[] { IntrinsicTypes.Uint }.Union(IntrinsicTypes.AllUintVectorTypes).ToArray()));

            allFunctions.AddRange(Create3(
                "faceforward",
                "Flips the surface-normal (if needed) to face in a direction opposite to i; returns the result in n.",
                IntrinsicTypes.AllFloatVectorTypes,
                "n", "The resulting floating-point surface-normal vector.",
                "i", "A floating-point, incident vector that points from the view position to the shading position.",
                "ng", "A floating-point surface-normal vector."));

            allFunctions.AddRange(Create1(
                "firstbithigh",
                "Gets the location of the first set bit starting from the highest order bit and working downward, per component.",
                new [] { IntrinsicTypes.Int }.Union(IntrinsicTypes.AllIntVectorTypes).Union(new[] { IntrinsicTypes.Uint }).Union(IntrinsicTypes.AllUintVectorTypes).ToArray(),
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "firstbitlow",
                "Returns the location of the first set bit starting from the lowest order bit and working upward, per component.",
                new[] { IntrinsicTypes.Int }.Union(IntrinsicTypes.AllIntVectorTypes).Union(new[] { IntrinsicTypes.Uint }).Union(IntrinsicTypes.AllUintVectorTypes).ToArray(),
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "floor",
                "Returns the largest integer that is less than or equal to the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create3(
                "fma",
                "Returns the double-precision fused multiply-addition of a * b + c.",
                IntrinsicTypes.AllDoubleTypes,
                "a", "The first value in the fused multiply-addition.",
                "b", "The second value in the fused multiply-addition.",
                "c", "The third value in the fused multiply-addition."));

            allFunctions.AddRange(Create2(
                "fmod",
                "Returns the floating-point remainder of x/y.",
                IntrinsicTypes.AllFloatTypes,
                "x", "The floating-point dividend.",
                "y", "The floating-point divisor."));

            allFunctions.AddRange(Create1(
                "frac",
                "Returns the fractional (or decimal) part of x; which is greater than or equal to 0 and less than 1.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create2(
                "frexp",
                "Returns the mantissa and exponent of the specified floating-point value.",
                IntrinsicTypes.AllFloatTypes,
                "x", "The specified floating-point value. If the x parameter is 0, this function returns 0 for both the mantissa and the exponent.",
                "exp", "The returned exponent of the x parameter.",
                overrideParameterDirection2: ParameterDirection.Out));

            allFunctions.AddRange(Create1(
                "fwidth",
                "Returns the absolute value of the partial derivatives of the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.Add(Create0(
                "GetRenderTargetSampleCount",
                "Gets the number of samples for a render target.",
                IntrinsicTypes.Uint));

            allFunctions.AddRange(Create1(
                "GetRenderTargetSamplePosition",
                "Gets the sampling position (x,y) for a given sample index.",
                new[] { IntrinsicTypes.Int },
                "index", "A zero-based sample index.",
                IntrinsicTypes.Float2));

            allFunctions.Add(Create0(
                "GroupMemoryBarrier",
                "Blocks execution of all threads in a group until all group shared accesses have been completed.",
                IntrinsicTypes.Void));

            allFunctions.Add(Create0(
                "GroupMemoryBarrierWithGroupSync",
                "Blocks execution of all threads in a group until all group shared accesses have been completed and all threads in the group have reached this call.",
                IntrinsicTypes.Void));

            allFunctions.AddRange(Create2(
                "InterlockedAdd",
                "Performs a guaranteed atomic add of value to the dest resource variable.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                IntrinsicTypes.Void));
            allFunctions.AddRange(Create3(
                "InterlockedAdd",
                "Performs a guaranteed atomic add of value to the dest resource variable.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                "originalValue", "The original input value.",
                IntrinsicTypes.Void,
                overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create2(
                "InterlockedAnd",
                "Performs a guaranteed atomic and.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                IntrinsicTypes.Void));
            allFunctions.AddRange(Create3(
                "InterlockedAnd",
                "Performs a guaranteed atomic and.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                "originalValue", "The original input value.",
                IntrinsicTypes.Void,
                overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create4(
                "InterlockedCompareExchange",
                "Atomically compares the destination with the comparison value. If they are identical, the destination is overwritten with the input value. The original value is set to the destination's original value.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "compareValue", "The comparison value.",
                "value", "The input value.",
                "originalValue", "The original value.",
                IntrinsicTypes.Void,
                overrideParameterDirection4: ParameterDirection.Out));

            allFunctions.AddRange(Create3(
                "InterlockedCompareStore",
                "Atomically compares the destination to the comparison value. If they are identical, the destination is overwritten with the input value.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "compareValue", "The comparison value.",
                "value", "The input value.",
                IntrinsicTypes.Void));

            allFunctions.AddRange(Create3(
                "InterlockedExchange",
                "Assigns value to dest and returns the original value.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                "originalValue", "The original input value.",
                IntrinsicTypes.Void,
                overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create2(
                "InterlockedMax",
                "Performs a guaranteed atomic max.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                IntrinsicTypes.Void));
            allFunctions.AddRange(Create3(
                "InterlockedMax",
                "Performs a guaranteed atomic max.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                "originalValue", "The original input value.",
                IntrinsicTypes.Void,
                overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create2(
                "InterlockedMin",
                "Performs a guaranteed atomic min.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                IntrinsicTypes.Void));
            allFunctions.AddRange(Create3(
                "InterlockedMin",
                "Performs a guaranteed atomic min.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                "originalValue", "The original input value.",
                IntrinsicTypes.Void,
                overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create2(
                "InterlockedOr",
                "Performs a guaranteed atomic or.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                IntrinsicTypes.Void));
            allFunctions.AddRange(Create3(
                "InterlockedOr",
                "Performs a guaranteed atomic or.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                "originalValue", "The original input value.",
                IntrinsicTypes.Void,
                overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create2(
                "InterlockedXor",
                "Performs a guaranteed atomic xor.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                IntrinsicTypes.Void));
            allFunctions.AddRange(Create3(
                "InterlockedXor",
                "Performs a guaranteed atomic xor.",
                new[] { IntrinsicTypes.Int, IntrinsicTypes.Uint },
                "dest", "The destination address.",
                "value", "The input value.",
                "originalValue", "The original input value.",
                IntrinsicTypes.Void,
                overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create1(
                "isfinite",
                "Determines if the specified floating-point value is finite.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value.",
                IntrinsicTypes.AllBoolTypes));

            allFunctions.AddRange(Create1(
                "isinf",
                "Determines if the specified value is infinite.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value.",
                IntrinsicTypes.AllBoolTypes));

            allFunctions.AddRange(Create1(
                "isnan",
                "Determines if the specified value is NAN or QNAN.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value.",
                IntrinsicTypes.AllBoolTypes));

            allFunctions.AddRange(Create2(
                "ldexp",
                "Determines if the specified value is NAN or QNAN.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value.",
                "exp", "The specified exponent."));

            allFunctions.AddRange(Create1(
                "length",
                "Returns the length of the specified floating-point vector.",
                IntrinsicTypes.AllFloatVectorTypes,
                "value", "The specified floating-point vector.",
                IntrinsicTypes.Float));

            allFunctions.AddRange(Create3(
                "lerp",
                "Performs a linear interpolation.",
                IntrinsicTypes.AllFloatVectorTypes,
                "x", "The first floating-point value.",
                "y", "The second floating-point value.",
                "s", "A value that linearly interpolates between the x parameter and the y parameter."));

            allFunctions.AddRange(Create3(
                "lit",
                "Returns a lighting coefficient vector.",
                new[] { IntrinsicTypes.Float },
                "nDotL", "The dot product of the normalized surface normal and the light vector.",
                "nDotH", "The dot product of the half-angle vector and the surface normal.",
                "m", "A specular exponent.",
                IntrinsicTypes.Float4));

            allFunctions.AddRange(Create1(
                "log",
                "Returns the base-e logarithm of the specified value.",
                IntrinsicTypes.AllFloatVectorTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "log10",
                "Returns the base-10 logarithm of the specified value.",
                IntrinsicTypes.AllFloatVectorTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "log2",
                "Returns the base-2 logarithm of the specified value.",
                IntrinsicTypes.AllFloatVectorTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create3(
                "mad",
                "Performs an arithmetic multiply/add operation on three values.",
                IntrinsicTypes.AllNumericTypes,
                "m", "The multiplication value.",
                "a", "The first addition value.",
                "b", "The second addition value."));

            allFunctions.AddRange(Create2(
                "max",
                "Selects the greater of x and y.",
                IntrinsicTypes.AllFloatTypes.Union(IntrinsicTypes.AllIntTypes).ToArray(),
                "x", "The x input value.",
                "y", "The y input value."));

            allFunctions.AddRange(Create2(
                "min",
                "Selects the lesser of x and y.",
                IntrinsicTypes.AllFloatTypes.Union(IntrinsicTypes.AllIntTypes).ToArray(),
                "x", "The x input value.",
                "y", "The y input value."));

            allFunctions.AddRange(Create2(
                "modf",
                "Splits the value x into fractional and integer parts, each of which has the same sign as x.",
                IntrinsicTypes.AllFloatTypes.Union(IntrinsicTypes.AllIntTypes).ToArray(),
                "x", "The x input value.",
                "ip", "The integer portion of x.",
                overrideParameterDirection2: ParameterDirection.Out));

            allFunctions.AddRange(Create3(
                "msad4",
                "Compares a 4-byte reference value and an 8-byte source value and accumulates a vector of 4 sums. Each sum corresponds to the masked sum of absolute differences of a different byte alignment between the reference value and the source value.",
                new[] { IntrinsicTypes.Uint },
                "reference", "The reference array of 4 bytes in one uint value.",
                "source", "The source array of 8 bytes in two uint2 values.",
                "accum", "A vector of 4 values. msad4 adds this vector to the masked sum of absolute differences of the different byte alignments between the reference value and the source value.",
                IntrinsicTypes.Uint4,
                overrideParameterType2: IntrinsicTypes.Uint2,
                overrideParameterType3: IntrinsicTypes.Uint4));

            // mul overloads from https://msdn.microsoft.com/en-us/library/windows/desktop/bb509628(v=vs.85).aspx
            // overload 1:
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                new [] { IntrinsicTypes.Float, IntrinsicTypes.Int },
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector."));
            // overload 2:
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllFloatVectorTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                overrideParameterType1: IntrinsicTypes.Float));
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllIntVectorTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                overrideParameterType1: IntrinsicTypes.Int));
            // overload 3:
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllFloatMatrixTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                overrideParameterType1: IntrinsicTypes.Float));
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllIntMatrixTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                overrideParameterType1: IntrinsicTypes.Int));
            // overload 4:
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllFloatVectorTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                overrideParameterType2: IntrinsicTypes.Float));
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllIntVectorTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                overrideParameterType2: IntrinsicTypes.Int));
            // overload 5:
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllFloatVectorTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                IntrinsicTypes.Float));
            allFunctions.AddRange(Create2(
                "mul",
                "Multiplies x and y using matrix math. The inner dimension x-columns and y-rows must be equal.",
                IntrinsicTypes.AllIntVectorTypes,
                "x", "The x input value. If x is a vector, it treated as a row vector.",
                "y", "The y input value. If y is a vector, it treated as a column vector.",
                IntrinsicTypes.Int));
            // overload 6: TODO
            // overload 7: TODO
            // overload 8: TODO
            // overload 9: TODO

            allFunctions.AddRange(Create1(
                "noise",
                "Generates a random value using the Perlin-noise algorithm.",
                IntrinsicTypes.AllFloatVectorTypes,
                "value", "A floating-point vector from which to generate Perlin noise.",
                IntrinsicTypes.Float));

            allFunctions.AddRange(Create1(
                "normalize",
                "Normalizes the specified floating-point vector according to x / length(x).",
                IntrinsicTypes.AllFloatVectorTypes,
                "value", "The specified floating-point vector."));

            allFunctions.AddRange(Create2(
                "pow",
                "Returns the specified value raised to the specified power.",
                IntrinsicTypes.AllFloatTypes,
                "x", "The specified value.",
                "y", "The specified power."));

            // TODO: printf (requires string type and variadic arguments)

            allFunctions.Add(new FunctionDeclarationSymbol(
                "Process2DQuadTessFactorsAvg",
                "Generates the corrected tessellation factors for a quad patch.",
                IntrinsicTypes.Void,
                f => new[]
                {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float4),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float2),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float4, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out)
                }));

            allFunctions.Add(new FunctionDeclarationSymbol(
                "Process2DQuadTessFactorsMax",
                "Generates the corrected tessellation factors for a quad patch.",
                IntrinsicTypes.Void,
                f => new[]
                {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float4),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float2),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float4, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out)
                }));

            allFunctions.Add(new FunctionDeclarationSymbol(
                "Process2DQuadTessFactorsMin",
                "Generates the corrected tessellation factors for a quad patch.",
                IntrinsicTypes.Void,
                f => new[]
                {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float4),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float2),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float4, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out)
                }));

            allFunctions.Add(new FunctionDeclarationSymbol(
                "ProcessIsolineTessFactors",
                "Generates the rounded tessellation factors for an isoline.",
                IntrinsicTypes.Void,
                f => new[]
                {
                    new ParameterSymbol("rawDetailFactor", "The desired detail factor.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("rawDensityFactor", "The desired density factor.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("roundedDetailFactor", "The rounded detail factor clamped to a range that can be used by the tessellator.", f, IntrinsicTypes.Float, ParameterDirection.Out),
                    new ParameterSymbol("roundedDensityFactor", "The rounded density factor clamped to a rangethat can be used by the tessellator.", f, IntrinsicTypes.Float, ParameterDirection.Out)
                }));

            allFunctions.Add(new FunctionDeclarationSymbol(
                "ProcessQuadTessFactorsAvg",
                "Generates the corrected tessellation factors for a quad patch.",
                IntrinsicTypes.Void,
                f => new[]
                {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float4),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float4, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out)
                }));

            allFunctions.Add(new FunctionDeclarationSymbol(
               "ProcessQuadTessFactorsMax",
               "Generates the corrected tessellation factors for a quad patch.",
               IntrinsicTypes.Void,
               f => new[]
               {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float4),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float4, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out)
               }));

            allFunctions.Add(new FunctionDeclarationSymbol(
               "ProcessQuadTessFactorsMin",
               "Generates the corrected tessellation factors for a quad patch.",
               IntrinsicTypes.Void,
               f => new[]
               {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float4),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float4, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float2, ParameterDirection.Out)
               }));

            allFunctions.Add(new FunctionDeclarationSymbol(
               "ProcessTriTessFactorsAvg",
               "Generates the corrected tessellation factors for a tri patch.",
               IntrinsicTypes.Void,
               f => new[]
               {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float3),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float3, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float, ParameterDirection.Out)
               }));

            allFunctions.Add(new FunctionDeclarationSymbol(
               "ProcessTriTessFactorsMax",
               "Generates the corrected tessellation factors for a tri patch.",
               IntrinsicTypes.Void,
               f => new[]
               {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float3),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float3, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float, ParameterDirection.Out)
               }));

            allFunctions.Add(new FunctionDeclarationSymbol(
               "ProcessTriTessFactorsMin",
               "Generates the corrected tessellation factors for a tri patch.",
               IntrinsicTypes.Void,
               f => new[]
               {
                    new ParameterSymbol("rawEdgeFactors", "The edge tessellation factors, passed into the tessellator stage.", f, IntrinsicTypes.Float3),
                    new ParameterSymbol("insideScale", "The scale factor applied to the UV tessellation factors computed by the tessellation stage. The allowable range for insideScale is 0.0 to 1.0.", f, IntrinsicTypes.Float),
                    new ParameterSymbol("roundedEdgeTessFactors", "The rounded edge-tessellation factors calculated by the tessellator stage.", f, IntrinsicTypes.Float3, ParameterDirection.Out),
                    new ParameterSymbol("roundedInsideTessFactors", "The rounded tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float, ParameterDirection.Out),
                    new ParameterSymbol("unroundedInsideTessFactors", "The tessellation factors calculated by the tessellator stage for inside edges.", f, IntrinsicTypes.Float, ParameterDirection.Out)
               }));

            allFunctions.AddRange(Create1(
                "radians",
                "Converts the specified value from degrees to radians.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "rcp",
                "Calculates a fast, approximate, per-component reciprocal.",
                IntrinsicTypes.AllFloatTypes.Union(IntrinsicTypes.AllDoubleTypes).ToArray(),
                "value", "The input value."));

            allFunctions.AddRange(Create2(
                "reflect",
                "Returns a reflection vector using an incident ray and a surface normal.",
                IntrinsicTypes.AllFloatVectorTypes,
                "i", "A floating-point, incident vector.",
                "n", "A floating-point, normal vector."));

            allFunctions.AddRange(Create3(
                "refract",
                "Returns a refraction vector using an entering ray, a surface normal, and a refraction index.",
                IntrinsicTypes.AllFloatVectorTypes,
                "i", "A floating-point, ray direction vector.",
                "n", "A floating-point, surface normal vector.",
                "η", "A floating-point, refraction index scalar.",
                overrideParameterType3: IntrinsicTypes.Float));

            allFunctions.AddRange(Create1(
                "reversebits",
                "Reverses the order of the bits, per component.",
                new[] { IntrinsicTypes.Uint }.Union(IntrinsicTypes.AllUintVectorTypes).ToArray(),
                "value", "The input value."));

            allFunctions.AddRange(Create1(
                "round",
                "Rounds the specified value to the nearest integer.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
                "rsqrt",
                "Returns the reciprocal of the square root of the specified value.",
                IntrinsicTypes.AllFloatTypes,
                "value", "The specified value."));

            allFunctions.AddRange(Create1(
               "saturate",
               "Clamps the specified value within the range of 0 to 1.",
               IntrinsicTypes.AllFloatTypes,
               "value", "The specified value."));

            allFunctions.AddRange(Create1(
               "sign",
               "Returns the sign of x.",
               IntrinsicTypes.AllFloatTypes.Union(IntrinsicTypes.AllIntTypes).ToArray(),
               "value", "The input value.",
               IntrinsicTypes.AllIntTypes.Union(IntrinsicTypes.AllIntTypes).ToArray()));

            allFunctions.AddRange(Create1(
               "sin",
               "Returns the sine of the specified value.",
               IntrinsicTypes.AllFloatTypes,
               "value", "The specified value, in radians."));

            allFunctions.AddRange(Create3(
               "sincos",
               "Returns the sine and cosine of x.",
               IntrinsicTypes.AllFloatTypes,
               "value", "The specified value, in radians.",
               "s", "Returns the sine of x.",
               "c", "Returns the cosine of x.",
               IntrinsicTypes.Void,
               overrideParameterDirection2: ParameterDirection.Out,
               overrideParameterDirection3: ParameterDirection.Out));

            allFunctions.AddRange(Create1(
               "sinh",
               "Returns the hyperbolic sine of the specified value.",
               IntrinsicTypes.AllFloatTypes,
               "value", "The specified value, in radians."));

            allFunctions.AddRange(Create3(
               "smoothstep",
               "Returns a smooth Hermite interpolation between 0 and 1, if x is in the range [min, max].",
               IntrinsicTypes.AllFloatTypes,
               "min", "The minimum range of the x parameter.",
               "max", "The maximum range of the x parameter.",
               "x", "The specified value to be interpolated."));

            allFunctions.AddRange(Create1(
              "sqrt",
              "Returns the square root of the specified floating-point value, per component.",
              IntrinsicTypes.AllFloatTypes,
              "value", "The specified floating-point value."));

            allFunctions.AddRange(Create2(
              "step",
              "Compares two values, returning 0 or 1 based on which value is greater.",
              IntrinsicTypes.AllFloatTypes,
              "y", "The first floating-point value to compare.",
              "x", "The second floating-point value to compare."));

            allFunctions.AddRange(Create1(
              "tan",
              "Returns the tangent of the specified value.",
              IntrinsicTypes.AllFloatTypes,
              "value", "The specified value, in radians."));

            allFunctions.AddRange(Create1(
              "tanh",
              "Returns the hyperbolic tangent of the specified value.",
              IntrinsicTypes.AllFloatTypes,
              "value", "The specified value, in radians."));

            AllFunctions = allFunctions;
        }

        private static FunctionDeclarationSymbol Create0(string name, string documentation, TypeSymbol returnType)
        {
            return new FunctionDeclarationSymbol(
                name, documentation, returnType,
                f => new ParameterSymbol[0]);
        }

        private static IEnumerable<FunctionDeclarationSymbol> Create1(
            string name, string documentation, TypeSymbol[] types, 
            string parameterName, string parameterDocumentation, 
            params TypeSymbol[] overrideReturnTypes)
        {
            if (overrideReturnTypes.Length == 0)
                overrideReturnTypes = null;
            else if (overrideReturnTypes.Length == 1)
                overrideReturnTypes = Enumerable.Repeat(overrideReturnTypes[0], types.Length).ToArray();

            return types.Select((type, i) => new FunctionDeclarationSymbol(
                name, documentation, overrideReturnTypes?[i] ?? type,
                f => new []
                {
                    new ParameterSymbol(parameterName ?? "value", parameterDocumentation ?? "The specified value.", f, type)
                }));
        }

        private static IEnumerable<FunctionDeclarationSymbol> Create2(
            string name, string documentation, TypeSymbol[] types, 
            string parameterName1, string parameterDocumentation1, 
            string parameterName2, string parameterDocumentation2,
            TypeSymbol overrideReturnType = null,
            TypeSymbol overrideParameterType1 = null,
            TypeSymbol overrideParameterType2 = null,
            ParameterDirection overrideParameterDirection2 = ParameterDirection.In)
        {
            return types.Select(type => new FunctionDeclarationSymbol(
                name, documentation, overrideReturnType ?? type,
                f => new[]
                {
                    new ParameterSymbol(parameterName1, parameterDocumentation1, f, overrideParameterType1 ?? type),
                    new ParameterSymbol(parameterName2, parameterDocumentation2, f, overrideParameterType2 ?? type, overrideParameterDirection2)
                }));
        }

        private static IEnumerable<FunctionDeclarationSymbol> Create3(
            string name, string documentation, TypeSymbol[] types,
            string parameterName1, string parameterDocumentation1,
            string parameterName2, string parameterDocumentation2,
            string parameterName3, string parameterDocumentation3,
            TypeSymbol overrideReturnType = null,
            TypeSymbol overrideParameterType2 = null,
            TypeSymbol overrideParameterType3 = null,
            ParameterDirection overrideParameterDirection2 = ParameterDirection.In,
            ParameterDirection overrideParameterDirection3 = ParameterDirection.In)
        {
            return types.Select(type => new FunctionDeclarationSymbol(
                name, documentation, overrideReturnType ?? type,
                f => new[]
                {
                    new ParameterSymbol(parameterName1, parameterDocumentation1, f, type),
                    new ParameterSymbol(parameterName2, parameterDocumentation2, f, overrideParameterType2 ?? type, overrideParameterDirection2),
                    new ParameterSymbol(parameterName3, parameterDocumentation3, f, overrideParameterType3 ?? type, overrideParameterDirection3)
                }));
        }

        private static IEnumerable<FunctionDeclarationSymbol> Create4(
            string name, string documentation, TypeSymbol[] types,
            string parameterName1, string parameterDocumentation1,
            string parameterName2, string parameterDocumentation2,
            string parameterName3, string parameterDocumentation3,
            string parameterName4, string parameterDocumentation4,
            TypeSymbol overrideReturnType = null,
            ParameterDirection overrideParameterDirection4 = ParameterDirection.In)
        {
            return types.Select(type => new FunctionDeclarationSymbol(
                name, documentation, overrideReturnType ?? type,
                f => new[]
                {
                    new ParameterSymbol(parameterName1, parameterDocumentation1, f, type),
                    new ParameterSymbol(parameterName2, parameterDocumentation2, f, type),
                    new ParameterSymbol(parameterName3, parameterDocumentation3, f, type),
                    new ParameterSymbol(parameterName4, parameterDocumentation4, f, type, overrideParameterDirection4)
                }));
        }
    }
}