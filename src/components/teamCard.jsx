export default function TeamCard({ team, onEdit, onDelete }) {
  return (
    <div className="team-card">
      <h3>{team.name}</h3>
      <p>{team.city}</p>
      <span className="category-tag">{team.category}</span>

      <div className="actions">
        <button onClick={onEdit}>Editar</button>
        <button className="delete" onClick={onDelete}>Excluir</button>
      </div>
    </div>
  );
}
