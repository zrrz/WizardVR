using UnityEngine;
using System.Collections;

//This is a generic interface where T is a placeholder
//for a data type that will be provided by the 
//implementing class.
public interface IDamageable
{
  void Damage(float damage);
}