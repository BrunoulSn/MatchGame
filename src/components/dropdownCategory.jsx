export default function DropdownCategory({ category, setCategory }) {
  const list = [
    { key: "all", label: "🌐 Todos" },
    { key: "futebol", label: "⚽ Futebol" },
    { key: "basquete", label: "🏀 Basquete" },
    { key: "volei", label: "🏐 Vôlei" },
    { key: "corrida", label: "🏃 Corrida" },
  ];

  return (
    <select
      className="dropdown"
      value={category}
      onChange={(e) => setCategory(e.target.value)}
    >
      {list.map(c => (
        <option key={c.key} value={c.key}>{c.label}</option>
      ))}
    </select>
  );
}
