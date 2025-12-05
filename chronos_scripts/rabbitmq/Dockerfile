FROM rabbitmq:3.13-management

COPY chronos_rabbitmq_def.json /etc/rabbitmq/chronos_rabbitmq_def.json
COPY rabbitmq.conf /etc/rabbitmq/rabbitmq.conf

RUN chown rabbitmq:rabbitmq /etc/rabbitmq/chronos_rabbitmq_def.json
