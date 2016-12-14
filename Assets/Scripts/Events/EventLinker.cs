using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;
using System.Reflection.Emit;
public class EventLinker {

	//I HAVE BEEN SEARCHING FOREVER HOW TO RUNTIME CAST UNKNOWN TYPES AND IT WAS SO FUCKING OBVIOUS FUCK YOU
	public static T AssCast<T> (System.Object obj) where T : IEvent{
		var temp = (T)obj;
		//Debug.Log("WE SUCCESFULLY CASTED TO: " + typeof(T));
		return temp;
	}

	public static void LinkEvents(BaseObject obj, EventManager eventManager){
		var baseType = typeof(IEvent);

		//Load the .NET assembly related to our IEvent (all code we make in a project is usually compiled to the same assembly AFAIK)
	    var assembly = baseType.Assembly;

	    //Get all interfaces of type IEvent
		List<System.Type> IEvents = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(baseType) && t.IsInterface).ToList();
		foreach (Type eventInterface in IEvents)
		{
			//check if we implement interface eventInterface
			if(IsImplementationOf(obj.GetType(),eventInterface))
			{
				//Get all methods in the interface
				MethodInfo[] methods = eventInterface.GetMethods();

				//Iterate over the methods on the interface that is going to be subscribed to the delegate
				//Idealy we'd probably only have one method for each interface though
				foreach(MethodInfo method in methods)
				{
					//We cast our BaseObject to interface eventInterface, we know this is valid because of previous check
					var assCast = typeof(EventLinker).GetMethod("AssCast");
					object[] assParms = new object[] {obj};
					assCast = assCast.MakeGenericMethod(eventInterface);
					IEvent implObj = assCast.Invoke(null, assParms) as IEvent;

					//Get the type for each argument/parameter in that method
					List<Type> paramTypes = method.GetParameters().Select(p => p.ParameterType).ToList();

					//The method in our EventManager through which we subscribe our method through
					List<MethodInfo> subscribeMethods = typeof(EventManager).GetMethods().Where(t => t.Name =="SubscribeEvent").ToList();

					//Find the correct overloaded method depending on how many generic arguments it has
					//We use the one that has as many arguments as our own method
					MethodInfo subscriptionMethod = null;
					foreach (var z in subscribeMethods) {
						var par = z.GetParameters();
						foreach(var a in par){
							if(a.ParameterType.GetGenericArguments().Count() == paramTypes.Count){
								subscriptionMethod = z;
							}
						}
					}

					//Depending on the amount of arguments we have to construct a delegate out of our method so we can pass it as an argument to the subscription method
					//Also, we have to call the subscription method through reflection because we cant specify type at compile time
					//This is ugly I am so sorry
					if (paramTypes.Count > 3) {
						throw new System.ArgumentException(method.Name + " has too many parameters, please cut down to a maximum of 3!");
					}
					else if (paramTypes.Count > 2) {
						//Create a new open generic type
						Type action = typeof(Action<,,>);

						//turn it into a closed generic type by supplying the types of our method arguments
						Type[] typeArgs = {paramTypes[0], paramTypes[1], paramTypes[2]};
						Type generic = action.MakeGenericType(typeArgs);

						//turn the whole thing into a new delegate of our new type Action<x,y,z> pointing to the implObject instance,
						//and the name of the method that we know will be there because it implements the interface
						//final two bool args are just if we are not case sensitive and if we throw an exception on failure
						var result = Delegate.CreateDelegate(generic, implObj, method.Name, false, true);

						//add our delegate to an array that will be supplied as argument when invoking through reflection
						object[] parametersArray = new object[] {result};

						//Build and invoke a reflected version of EventManager.SubscribeEvent<>();
						//With as many arguments as would be there had we called it normally
						subscriptionMethod = subscriptionMethod.MakeGenericMethod(eventInterface, paramTypes[0], paramTypes[1], paramTypes[2]);
						subscriptionMethod.Invoke(eventManager, parametersArray);
					}
					else if (paramTypes.Count > 1) {
						Type action = typeof(Action<,>);
						Type[] typeArgs = {paramTypes[0], paramTypes[1]};
						Type generic = action.MakeGenericType(typeArgs);
						var result = Delegate.CreateDelegate(generic, implObj, method.Name, false, true);
						object[] parametersArray = new object[] {result};
						subscriptionMethod = subscriptionMethod.MakeGenericMethod(eventInterface, paramTypes[0], paramTypes[1]);
						subscriptionMethod.Invoke(eventManager, parametersArray);
					}
					else if (paramTypes.Count > 0) {
						Type action = typeof(Action<>);
						Type[] typeArgs = {paramTypes[0]};
						Type generic = action.MakeGenericType(typeArgs);
						var result = Delegate.CreateDelegate(generic, implObj, method.Name, false, true);
						object[] parametersArray = new object[] {result};
						subscriptionMethod = subscriptionMethod.MakeGenericMethod(eventInterface, paramTypes[0]);
						subscriptionMethod.Invoke(eventManager, parametersArray);
					}
					else if (paramTypes.Count == 0) {
						var result = Delegate.CreateDelegate(typeof(System.Action), implObj, method.Name, false, true);
						object[] parametersArray = new object[] {result};
						subscriptionMethod = subscriptionMethod.MakeGenericMethod(eventInterface);
						subscriptionMethod.Invoke(eventManager, parametersArray);
					}
				}
			}
		}
	}

	//Does the object implement a given interface
	public static bool IsImplementationOf(Type checkMe, Type forMe)
	{
	    if (forMe.IsGenericTypeDefinition)
	        return checkMe.GetInterfaces().Select(i =>
	        {
	            if (i.IsGenericType)
	                return i.GetGenericTypeDefinition();

	            return i;
	        }).Any(i => i == forMe);

	    return forMe.IsAssignableFrom(checkMe);
	}
}
