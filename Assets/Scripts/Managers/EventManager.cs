using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using System;
using FullInspector;
public class EventManager : MonoBehaviour {
	//Dictionary were we link up every IEvent interface with a delegate
	//Which is actually an Action<>, but we upcast to delegate so we can have different amounts of arguments (Action<X>, Action<X,Y,Z>, etc).
	private Dictionary<System.RuntimeTypeHandle, System.Delegate> delegateDict = new Dictionary<System.RuntimeTypeHandle, System.Delegate>();
	private Dictionary<System.RuntimeTypeHandle, EventVisualizer> eventImplementers = new Dictionary<System.RuntimeTypeHandle,EventVisualizer>();
    public List<EventVisualizer> eventVisualizer = new List<EventVisualizer>();
	//INVOKE EVENTS

	//First overload takes no arguments and is fast and nice
	public void InvokeEvent<TEvent>() where TEvent : IEvent{
		System.Delegate del;
		//Throw our runtimeTypeHandle of TEvent against the dictionary and hope for a matching delegate
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			//Downcast the delegate back to Action, allowing us to invoke it much faster than Delelegate.DynamicInvoke()
			var act = del as System.Action;
			if(act!=null){
				//Invoke all subscribed methods
				act();
			}
		}
	}

	//These are slower because Dynamicinvoke has to use reflection behind the scenes to figure out what the arguments are, and will just fail if they're wrong
	//Totally generic delegates can only be invoked by DynamicInvoke, whereas Action<> delegates can be called almost as fast as a normal method.
	//It's no problem as long as you don't call it every frame and stuff like that, but it's a shame when our delegate was originally a nice Action<>
	public void InvokeEvent<TEvent>(System.Object arg) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			del.DynamicInvoke(arg);
		}
	}

	public void InvokeEvent<TEvent>(System.Object arg, System.Object arg2) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			del.DynamicInvoke(arg,arg2);
		}
	}

	public void InvokeEvent<TEvent>(System.Object arg, System.Object arg2, System.Object arg3) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			del.DynamicInvoke(arg,arg2,arg3);
		}
	}

	//we can make it faster by supplying the type as a generic argument, since we can then cast our delegate its original Action type
	//It's more annoying because you use it like: InvokeEvent<IEvent, BlaBlaType1, BlaBlaType2, BlaBlaType3> (x, y, z); etc
	public void InvokeEventFast<TEvent, T1>(T1 arg) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			var act = del as System.Action<T1>;
			if(act!=null){
				act(arg);
			}
		}
	}

	public void InvokeEventFast<TEvent, T1, T2>(T1 arg, T2 arg2) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			var act = del as System.Action<T1,T2>;
			if(act!=null){
				act(arg, arg2);
			}
		}
	}

	public void InvokeEventFast<TEvent, T1, T2,T3>(T1 arg, T2 arg2, T3 arg3) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			var act = del as System.Action<T1,T2,T3>;
			if(act!=null){
				act(arg, arg2, arg3);
			}
		}
	}


	//SUBSCRIBE TO EVENT
	public void SubscribeEvent<TEvent>(System.Action method) where TEvent : IEvent{
		var handle = typeof(TEvent).TypeHandle;
		System.Delegate del;
		if(delegateDict.TryGetValue(handle, out del)){
			//Delegates are copies and not refs it seems,
			//so when we modify our invocationlist with += or -= we have to overrride the old dictionary value with our new delegate to actually update it
			var act = del as System.Action;
			act += method;

			delegateDict[handle] = act;
			VisualizeSubscribers<TEvent>(method.Target as MonoBehaviour);
		}
	}

	public void SubscribeEvent<TEvent, T1>(System.Action<T1> method) where TEvent : IEvent{
		var handle = typeof(TEvent).TypeHandle;
		System.Delegate del;
		if(delegateDict.TryGetValue(handle, out del)){
			var act = del as System.Action<T1>;
			act += method;

			delegateDict[handle] = act;
			VisualizeSubscribers<TEvent>(method.Target as MonoBehaviour);
		}
	}
	public void SubscribeEvent<TEvent, T1, T2>(System.Action<T1,T2> method) where TEvent : IEvent{
		var handle = typeof(TEvent).TypeHandle;
		System.Delegate del;
		if(delegateDict.TryGetValue(handle, out del)){
			var act = del as System.Action<T1,T2>;

			act += method;

			delegateDict[handle] = act;
			VisualizeSubscribers<TEvent>(method.Target as MonoBehaviour);
		}
	}
	public void SubscribeEvent<TEvent, T1, T2, T3>(System.Action<T1,T2,T3> method) where TEvent : IEvent{
		var handle = typeof(TEvent).TypeHandle;
		System.Delegate del;
		if(delegateDict.TryGetValue(handle, out del)){
			var act = del as System.Action<T1,T2,T3>;
			act += method;

			delegateDict[handle] = act;
			VisualizeSubscribers<TEvent>(method.Target as MonoBehaviour);
		}
	}

	void VisualizeSubscribers<TEvent>(MonoBehaviour owner){
		if(owner!=null){
			eventImplementers[typeof(TEvent).TypeHandle].implementations.Add(owner);
		}
	}

	//UNSUB to an event;
	public void UnSubscribeEvent<TEvent>(System.Action method) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			//Delegates are copies and not refs it seems,
			//so when we modify our invocationlist with += or -= we have to overrride the old dictionary value with our new delegate to actually update it
			var act = del as System.Action;
			act -= method;
			delegateDict[typeof(TEvent).TypeHandle] = act;
		}
	}
	public void UnSubscribeEvent<TEvent, T>(System.Action<T> method) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			var act = del as System.Action<T>;
			act -= method;
			delegateDict[typeof(TEvent).TypeHandle] = act;
		}
	}
	public void UnSubscribeEvent<TEvent, T1, T2>(System.Action<T1,T2> method) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			var act = del as System.Action<T1, T2>;
			act -= method;
			delegateDict[typeof(TEvent).TypeHandle] = act;
		}
	}
	public void UnSubscribeEvent<TEvent, T1, T2, T3>(System.Action<T1,T2,T3> method) where TEvent : IEvent{
		System.Delegate del;
		if(delegateDict.TryGetValue(typeof(TEvent).TypeHandle, out del)){
			var act = del as System.Action<T1, T2, T3>;
			act -= method;
			delegateDict[typeof(TEvent).TypeHandle] = act;
		}
	}

	//All these are just there to supply a correct empty Action<> delegate for our dictionary
	//This will resolve to something that the compiler will accept as an Action with the correct argument types while actually being null
	//This isn't a problem because we want the Action to be null until we do += on it
	public Action DynDel(){
		return (Action)delegate{} ;
	}
	public Action<T> DynDel1<T>(){
		return (Action<T>)delegate{} ;
	}
	public Action<T,T2> DynDel2<T,T2>(){
		return (Action<T,T2>)delegate{} ;
	}
	public Action<T,T2,T3> DynDel3<T,T2,T3>(){
		return (Action<T,T2,T3>)delegate{} ;
	}
	public Action<T,T2,T3,T4> DynDel4<T,T2,T3,T4>(){
		return (Action<T,T2,T3,T4>)delegate{} ;
	}

	//Jesus christ killmee
	//This automatically creates an Action delegate for every event interface we have
	void SetupActions(){
		var baseType = typeof(IEvent);
	    var assembly = baseType.Assembly;
	    //Make a list of every interface that implements IEvent (which is just an empty interface so we can identify our events)
		List<System.Type> IEvents = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(baseType) && t.IsInterface).ToList();
		foreach (var x in IEvents) {

			//we add every interface to a public dictionary to get an overview of who is implementing what
			if(x.ToString()!=""){
				var evViz = new EventVisualizer (x.ToString());
				eventImplementers.Add(x.TypeHandle, evViz);
				eventVisualizer.Add(evViz);
			}


			var methods = x.GetMethods();
			//every event interface should have exactly one method, but just in case we make sure
			if(methods.Length>0){
				var par = methods[0].GetParameters();
				List<Type> methodParamTypes = new List<Type>();
				foreach (var y in par) {
					methodParamTypes.Add(y.ParameterType);
				}
				//Dictionary.Add() wants to compile-time verifiable types
				//But we can call it through reflection to get around that
				MethodInfo dictMethod = typeof(IDictionary).GetMethod("Add");
				MethodInfo dictMethodGeneric = dictMethod;
				MethodInfo generic;

				if (methodParamTypes.Count > 3) {
					Debug.Log("Oh jeez, " +x+ " has a lot of parameters. m-m-maybe too many dunno.");
				}
				else if (methodParamTypes.Count > 2) {
					//Generate an Action delegate with the amount of generic parameters it needs
					MethodInfo dynMeth = typeof(EventManager).GetMethod("DynDel3");
					generic = dynMeth.MakeGenericMethod(methodParamTypes[0], methodParamTypes[1], methodParamTypes[2]);
					//The generic method returns the action that we are going to supply to our dictionary
					var output = generic.Invoke(this, null);
					object[] parametersArray = new object[] {x.TypeHandle,output};
					//Invoke the Dictionary.Add() method through reflection and supply our parameters
					dictMethodGeneric.Invoke(this.delegateDict,parametersArray);

				}
				else if (methodParamTypes.Count > 1) {
					MethodInfo dynMeth = typeof(EventManager).GetMethod("DynDel2");
					generic = dynMeth.MakeGenericMethod(methodParamTypes[0], methodParamTypes[1]);
					var output = generic.Invoke(this, null);
					object[] parametersArray = new object[] {x.TypeHandle,output};
					dictMethodGeneric.Invoke(this.delegateDict,parametersArray);

				}
				else if (methodParamTypes.Count > 0) {
					MethodInfo dynMeth = typeof(EventManager).GetMethod("DynDel1");
					generic = dynMeth.MakeGenericMethod(methodParamTypes[0]);
					var output = generic.Invoke(this, null);
					object[] parametersArray = new object[] {x.TypeHandle,output};
					dictMethodGeneric.Invoke(this.delegateDict,parametersArray);
				}
				else if (methodParamTypes.Count == 0) {
					delegateDict.Add(x.TypeHandle, DynDel());
				}
			}
		}
	}

	void Awake(){
		SetupActions();
		if(FindObjectOfType<SettingsControl>()==null){
			gameObject.AddComponent<SettingsControl>();
		}
	}
}

[System.Serializable]
public class EventVisualizer{
	[HideInInspector]
	public string name;
	public List<MonoBehaviour> implementations;


	public EventVisualizer(string name){
		this.name = name;
		implementations = new List<MonoBehaviour>();
	}
}
