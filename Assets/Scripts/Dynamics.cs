using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Dynamics
{

    public class Dynamics : MonoBehaviour
    {
        public Team Team => _team;
        [SerializeField] private Team _team;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private int type;


        private float _attackRange = 1f;
        private float _rayDistance = 1.0f;
        private float _stoppingDistance = 3f;
        private float speed = 5f;

        private Vector3 _destination;
        private Quaternion _desiredRotation;
        private Vector3 _direction;
        private Dynamics _target;
        private State _currentState;
        private bool stop=false;

        //private PredatorBrain brainPredator;

        void Awake()
        {
            /* if(gameObject.GetComponent<Dynamics>().Team == 0)
                   brainPredator = gameObject.GetComponent<PredatorBrain>(); 
                   type = 0; */
                   //get the dna script attached to the Predator agent
        }
        private void Update()
        {
            if(speed ==  0)
                speed = 1;
            if(gameObject.GetComponent<Dynamics>().type == 0)
                    speed = gameObject.GetComponent<PredatorBrain>().dna.GetGene(0);
                    //Debug.Log("DNA obtained");

            switch (_currentState)
            {
                case State.Wander:
                    {
                        if (NeedsDestination())
                        {
                            GetDestination();
                        }

                        transform.rotation = _desiredRotation;

                        transform.Translate(Vector3.forward * Time.deltaTime * speed);

                        var rayColor = IsPathBlocked() ? Color.red : Color.green;
                        Debug.DrawRay(transform.position, _direction * _rayDistance, rayColor);

                        while (IsPathBlocked())
                        {
                            ///Debug.Log("Path Blocked");
                            GetDestination();
                        }

                        var targetToAggro = CheckForAggro();//returns the object transform if hit returns true
                        if (targetToAggro != null)
                        {
                            _target = targetToAggro.GetComponent<Dynamics>();
                            _currentState = State.Chase;
                        }

                        break;
                    }
                case State.Chase:
                    {
                        if (_target == null)
                        {
                            _currentState = State.Wander;
                            return;
                        }

                        transform.LookAt(_target.transform);
                        transform.Translate(Vector3.forward * Time.deltaTime * 5f);

                        if (Vector3.Distance(transform.position, _target.transform.position) < _attackRange)
                        {
                            _currentState = State.Attack;
                        }
                        break;
                    }
                case State.Attack:
                    {
                        if (_target != null && _target.gameObject.GetComponent<Dynamics>().type == 1)
                        {
                            gameObject.GetComponent<PredatorBrain>().hungry = false;//Time the predator is hungry being set false by this event
                            Destroy(_target.gameObject);
                            //Debug.Log("Prey Eaten");
                        }

                        // play laser beam

                        _currentState = State.Wander;
                        break;
                    }
            }
        }

        private bool IsPathBlocked()
        {
            Ray ray = new Ray(transform.position, _direction);
            var hitSomething = Physics.RaycastAll(ray, _rayDistance, _layerMask);
            return hitSomething.Any();
        }

        private void GetDestination()
        {
            Vector3 testPosition = (transform.position + (transform.forward * 4f)) +
                                   new Vector3(UnityEngine.Random.Range(-4.5f, 4.5f), 0f,
                                       UnityEngine.Random.Range(-4.5f, 4.5f));

            _destination = new Vector3(testPosition.x, 1f, testPosition.z);

            _direction = Vector3.Normalize(_destination - transform.position);
            _direction = new Vector3(_direction.x, 0f, _direction.z);
            _desiredRotation = Quaternion.LookRotation(_direction);
        }

        private bool NeedsDestination()
        {
            if(stop == true)
                  return false;
            if (_destination == Vector3.zero)
                return true;

            var distance = Vector3.Distance(transform.position, _destination);
            if (distance <= _stoppingDistance)
            {
                return true;
            }

            return false;
        }



        Quaternion startingAngle = Quaternion.AngleAxis(-60, Vector3.up);
        Quaternion stepAngle = Quaternion.AngleAxis(5, Vector3.up);

        private Transform CheckForAggro()
        {
            float aggroRadius = 3f;

            RaycastHit hit;
            var angle = transform.rotation * startingAngle;
            var direction = angle * Vector3.forward;
            var pos = transform.position;
            for (var i = 0; i < 24; i++)
            {
                if (Physics.Raycast(pos, direction, out hit, aggroRadius))
                {
                    var dynamic = hit.collider.GetComponent<Dynamics>();
                    if (dynamic != null && dynamic.Team != gameObject.GetComponent<Dynamics>().Team)
                    {
                        Debug.DrawRay(pos, direction * hit.distance, Color.red);
                        return dynamic.transform;
                    }
                    else
                    {
                        Debug.DrawRay(pos, direction * hit.distance, Color.yellow);
                    }
                }
                else
                {
                    Debug.DrawRay(pos, direction * aggroRadius, Color.white);
                }
                direction = stepAngle * direction;
            }

            return null;
        }
    }

    public enum Team
    {
        Predator,
        Prey
    }

    public enum State
    {
        Wander,
        Chase,
        Attack
    }

}