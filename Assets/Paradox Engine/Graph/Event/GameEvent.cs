/** GPL v3 License
 * 
 * Copyright © 2019 DeadlySmile
 * Copyright © 2019 Lucas Leonardo Conti
 * 
 * GameEvent.cs is part of ParadoxFramework.
 * 
 * ParadoxFramework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * ParadoxFramework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with ParadoxFramework. If not, see <http://www.gnu.org/licenses/>.
 **/

using System.Collections.Generic;
using UnityEngine;

namespace ParadoxFramework.Events
{
    [CreateAssetMenu(menuName ="Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> _listeners = new List<GameEventListener>();

        /// <summary>
        /// Suscribe a GameEventListener to the GameEvent.
        /// </summary>
        /// <param name="listener">Listener</param>
        public void SuscribeListener(GameEventListener listener) => _listeners.Add(listener);
        /// <summary>
        /// Unsuscribe a GameEventListener to the GameEvent.
        /// </summary>
        /// <param name="listener">Listener</param>
        public void UnsuscribeListener(GameEventListener listener) => _listeners.Remove(listener);

        /// <summary>
        /// Execute all Events.
        /// </summary>
        public void ExecuteEvent()
        {
            for (int i = 0; i < _listeners.Count; i++)
                _listeners[i].OnExecuteEvent();
        }
    }
}