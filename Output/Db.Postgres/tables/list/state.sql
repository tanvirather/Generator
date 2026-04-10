create table if not exists list.state (
    id int not null,
    updated_by_id uuid not null,
    updated timestamp not null default current_timestamp,
    sort_order int,
    text varchar(50)
);
