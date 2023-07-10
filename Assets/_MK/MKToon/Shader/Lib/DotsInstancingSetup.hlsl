//////////////////////////////////////////////////////
// MK Toon Dots Instancing Setup					//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

#ifndef MK_TOON_DOTS_INSTANCING_SETUP
	#if UNITY_VERSION >= 202310 && SHADER_TARGET >= 35
		#include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
	#elif UNITY_VERSION >= 202220 && SHADER_TARGET >= 35
		#pragma multi_compile __ DOTS_INSTANCING_ON
	#elif UNITY_VERSION >= 202030 && SHADER_TARGET >= 45
		#pragma multi_compile __ DOTS_INSTANCING_ON
	#endif
#endif