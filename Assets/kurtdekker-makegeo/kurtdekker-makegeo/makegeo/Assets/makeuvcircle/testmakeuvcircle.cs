﻿/*
	The following license supersedes all notices in the source code.

	Copyright (c) 2019 Kurt Dekker/PLBM Games All rights reserved.

	http://www.twitter.com/kurtdekker

	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are
	met:

	Redistributions of source code must retain the above copyright notice,
	this list of conditions and the following disclaimer.

	Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.

	Neither the name of the Kurt Dekker/PLBM Games nor the names of its
	contributors may be used to endorse or promote products derived from
	this software without specific prior written permission.

	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
	IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
	TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
	PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
	TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
	PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
	LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testmakeuvcircle : MonoBehaviour
{
	// provide some materials; this will loop through them
	public Material[] materials;
	
	void Start ()
	{
		int n = 0;

		if (materials == null || materials.Length == 0)
		{
			materials = new Material[1];	// make a blank one
		}

		// let's make a very low poly one
		GameObject go = MakeUVCircle.Create (Vector3.one, AxisDirection.ZMINUS, 10);
		go.GetComponent<MeshRenderer> ().material = materials [n % materials.Length];
		go.transform.position = Vector3.left * 1.4f;
		SpinMeY.Attach(go);
		n++;

		// and now one that looks good
		go = MakeUVCircle.Create (Vector3.one, AxisDirection.ZPLUS, 32);
		go.GetComponent<MeshRenderer> ().material = materials [n % materials.Length];
		go.transform.position = Vector3.right * 1.4f;
		SpinMeY.Attach(go);
		n++;

		// and finally a SUPER-fine one, coincidentally also double-sided
		go = MakeUVCircle.Create (Vector3.one, AxisDirection.ZMINUS, 1024, true);
		go.GetComponent<MeshRenderer> ().material = materials [n % materials.Length];
		go.transform.position = Vector3.up * 1.4f;
		SpinMeY.Attach(go);
		n++;

		// a flat one, double-sided
		go = MakeUVCircle.Create (Vector3.one, AxisDirection.YPLUS, 32, true);
		go.GetComponent<MeshRenderer> ().material = materials [n % materials.Length];
		go.transform.position = Vector3.down * 1.0f;
		SpinMeZ.Attach(go);
		n++;

		// an inverted one high where we can see it
		go = MakeUVCircle.Create (Vector3.one, AxisDirection.YMINUS, 32);
		go.GetComponent<MeshRenderer> ().material = materials [n % materials.Length];
		go.transform.position = Vector3.up * 4.0f;
		SpinMeY.Attach(go);
		n++;

		// a nice simple five-pointed star
		go = MakeUVCircle.Create(Vector3.one, AxisDirection.ZMINUS, 10, CyclicRadiusModifiers: new float[] { 1.0f, 0.5f } );
		go.GetComponent<MeshRenderer>().material = materials[n % materials.Length];
		go.transform.position = Vector3.left * 3.4f + Vector3.up * 1.6f;
		n++;
	}
}
