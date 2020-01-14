Shader "RadarShader"
{
	Properties{
		//Colours
		_ColorPlayer("ColorPlayer", color) = (0.5, 0.5, 0.5, 0)
		_ColorMonster("ColorMonster", color) = (0.5, 0.5, 0.5, 0)

		//Large Pulse Monster Var
		_MonsterWidth("MonsterWidth", float) = 1.0
		_MonsterPos("MonsterPos", vector) = (0,0,0)
		_MonsterRadius("MonsterRadius", float) = 0.0

		//Large Pulse Player Var
		_PlayerWidth("PlayerWidth", float) = 1.0
		_PlayerPos("PlayerPos", vector) = (0,0,0)
		_PlayerRadius("PlayerRadius", float) = 0.0
		Time("Time", float) = 0.0

		//Local View Player
		_FadeAmmount("FadeAmmount", float) = 0.0
		_Width("Width", float) = 1.0
		_Center("Center", vector) = (0,0,0)
		_Radius("Radius", float) = 1.5
	}
		SubShader
		{
			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				//Colours
				float4 _ColorMonster;
				float4 _ColorPlayer;

				//Monster Large Pulse
				float3 _MonsterPos;
				float _MonsterRadius;
				float _MonsterWidth;

				//Player Large Pulse
				float3 _PlayerPos;
				float _PlayerRadius;
				float _PlayerWidth;
				float Time;

				//Player Local View
				float3 _Center;
				float _Radius;
				float _Width;
				float _FadeAmmount;

				struct v2f {
					float4 pos : SV_POSITION;
					float3 worldPos : TEXCOORD1;
				};

				v2f vert(appdata_base v) {
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					return o;
				}

				fixed4 frag(v2f i) : COLOR {
					float dist; float fade; float output = 0;
					float distPlayer; float fadePlayer; float outputPlayer = 0;
					float distMonster; float outputMonster = 0;
					
				//- dist is the distance between the players position and the position in the world (object) -//
				//- fade is calculated through dist + the normalised world pos and the devide by 20 is to tone down the brightness -//
				//- step(x,y) returns the larger value the fade is added to the return that is then times by dist or _radius -//
				//- depending on what is larger and then is times by time (time is a range from 1 to 0) -//

					dist = distance(_Center, i.worldPos);
					fade = (dist + normalize(i.worldPos)) / 20;
					output = ((step(_Radius - _Width, dist) + fade) * (step(dist, _Radius))) * Time;
					
					distPlayer = distance(_PlayerPos, i.worldPos);
					fadePlayer = (distPlayer - normalize(i.worldPos)) / _FadeAmmount;
					outputPlayer = (step(_PlayerRadius - _PlayerWidth, distPlayer) * step(distPlayer, _PlayerRadius) * (1 - step(distPlayer, _PlayerRadius - _PlayerWidth) * _PlayerWidth) - fadePlayer);

					distMonster = distance(_MonsterPos, i.worldPos);
					outputMonster = ((step(_MonsterRadius - _MonsterWidth, distMonster)) * (step(distMonster, _MonsterRadius)));

					if (outputMonster > output && outputMonster > outputPlayer)
					{
						return fixed4(((outputMonster)* _ColorMonster.r), ((outputMonster)* _ColorMonster.g), ((outputMonster)* _ColorMonster.b), 1.0);
					}
					else
					{
						if (outputPlayer > output)
							return fixed4(((outputPlayer)* _ColorPlayer.r), ((outputPlayer)* _ColorPlayer.g), ((outputPlayer)* _ColorPlayer.b), 1.0);
						else
							return fixed4(((output)* _ColorPlayer.r), ((output)* _ColorPlayer.g), ((output)* _ColorPlayer.b), 1.0);
					}
					
					// The colour is times by the output intensity
					//alpha is hard coded to 1
					return fixed4(0,0,0,1);
				}

				ENDCG
			}
	}
		FallBack "Diffuse"
}
